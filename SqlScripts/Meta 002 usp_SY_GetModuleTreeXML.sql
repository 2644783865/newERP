IF exists (select * from dbo.sysobjects where id = object_id(N'[usp_SY_GetModuleTreeXML]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure usp_SY_GetModuleTreeXML
GO

CREATE PROCEDURE usp_SY_GetModuleTreeXML
(
	@CompanyID int,
	@UserID int
)
AS
SET NOCOUNT ON
--权限
DECLARE @IsSystemAdmin bit,
	@IsClientAdmin bit

SET @IsSystemAdmin = 0
SET @IsClientAdmin = 0

IF EXISTS(SELECT 1 FROM SYtbSystemAdmin WHERE UserID = @UserID AND Active = 1)
	SET @IsSystemAdmin = 1

IF EXISTS(SELECT 1 FROM SYtbUser WHERE Enabled = 1 AND ID = @UserID AND IsClientAdmin = 1)
	SET @IsClientAdmin = 1

--debug
print @IsSystemAdmin
print @IsClientAdmin

CREATE TABLE #AvailabelNodes(ID int)

IF @IsSystemAdmin = 1
BEGIN
	INSERT INTO #AvailabelNodes(ID)
	SELECT ID
	FROM SYtbModule
END
ELSE IF @IsClientAdmin = 1
BEGIN
	INSERT INTO #AvailabelNodes(ID)
	SELECT ID
	FROM SYtbModule
	WHERE SYtbModule.Code NOT LIKE 'Admin%'
END
ELSE
BEGIN
	INSERT INTO #AvailabelNodes(ID)
	SELECT SYtbModule.ID
	FROM SYtbModule
	INNER JOIN SYtbModulePermissionRole
	ON SYtbModule.ID = SYtbModulePermissionRole.ModuleID
	INNER JOIN SYtbUserPermissionRole
	ON SYtbUserPermissionRole.PermissionRoleID = SYtbModulePermissionRole.PermissionRoleID
		AND SYtbUserPermissionRole.CompanyID = SYtbModulePermissionRole.CompanyID
	WHERE SYtbUserPermissionRole.UserID = @UserID
		AND SYtbUserPermissionRole.CompanyID = @CompanyID
		AND SYtbModule.Code NOT LIKE 'Admin%'

	--add parent
	INSERT INTO #AvailabelNodes(ID)
	SELECT SYtbModule.ID
	FROM SYtbModule
	WHERE SYtbModule.ParentID = 0
		AND NOT EXISTS(SELECT 1 FROM #AvailabelNodes AvailabelNodes
						WHERE AvailabelNodes.ID = SYtbModule.ID)
		AND EXISTS(SELECT 1 FROM #AvailabelNodes AvailabelNodes
					INNER JOIN SYtbModule SubNodes
					ON AvailabelNodes.ID = SubNodes.ID
					AND SubNodes.ParentID <> 0
					WHERE SubNodes.ParentID = SYtbModule.ID
				)
END

SELECT 
	[Text] = TreeNode.Name,
	[NodeID] = 'Node_' + CONVERT(nvarchar, TreeNode.ID),
	[Expanded] = CASE WHEN ISNULL(TreeNode.Expanded, 0) = 1 THEN 'true' ELSE 'false' END,
	[Highlight] = CASE WHEN TreeNode.Highlight = 1 THEN 'true' ELSE 'false' END,
	[NavigateUrl] = CASE WHEN TreeNode.Url IS NOT NULL THEN TreeNode.Url ELSE NULL END,
	[Icon] = TreeNode.Icon,
	(SELECT
			'Node_' + CONVERT(nvarchar, Level2TreeNode.ID) AS [TreeNode/@NodeID],
			CASE WHEN ISNULL(Level2TreeNode.Expanded, 0) = 1 THEN 'true' ELSE 'false' END AS [TreeNode/@Expanded],
			CASE WHEN Level2TreeNode.Highlight = 1 THEN 'true' ELSE 'false' END AS [TreeNode/@Highlight],
			Level2TreeNode.Name AS [TreeNode/@Text],
			CASE WHEN Level2TreeNode.Url IS NOT NULL THEN Level2TreeNode.Url ELSE NULL END AS [TreeNode/@NavigateUrl],
			Level2TreeNode.Icon AS [TreeNode/@Icon],

			(SELECT
					'Node_' + CONVERT(nvarchar, Level3TreeNode.ID) AS [TreeNode/@NodeID],
					CASE WHEN ISNULL(Level3TreeNode.Expanded, 0) = 1 THEN 'true' ELSE 'false' END AS [TreeNode/@Expanded],
					CASE WHEN Level3TreeNode.Highlight = 1 THEN 'true' ELSE 'false' END AS [TreeNode/@Highlight],
					Level3TreeNode.Name AS [TreeNode/@Text],
					CASE WHEN Level3TreeNode.Url IS NOT NULL THEN Level3TreeNode.Url ELSE NULL END AS [TreeNode/@NavigateUrl],
					Level3TreeNode.Icon AS [TreeNode/@Icon]
				FROM SYtbModule AS Level3TreeNode
				INNER JOIN #AvailabelNodes AvailabelNodes
				ON Level3TreeNode.ID = AvailabelNodes.ID
				WHERE Level3TreeNode.ParentID = Level2TreeNode.ID
				AND Level3TreeNode.Active = 1
				ORDER BY Level3TreeNode.DisplayOrder
				FOR XML PATH(''),TYPE
			) TreeNode

		FROM SYtbModule AS Level2TreeNode
		INNER JOIN #AvailabelNodes AvailabelNodes
		ON Level2TreeNode.ID = AvailabelNodes.ID
		WHERE Level2TreeNode.ParentID = TreeNode.ID
		AND Level2TreeNode.Active = 1
		ORDER BY Level2TreeNode.DisplayOrder
		FOR XML PATH(''),TYPE
		)
FROM SYtbModule AS TreeNode	
INNER JOIN #AvailabelNodes AvailabelNodes
ON TreeNode.ID = AvailabelNodes.ID
WHERE TreeNode.Active = 1
	AND TreeNode.ParentID = 0
ORDER BY TreeNode.DisplayOrder
FOR XML AUTO
--FOR XML AUTO, ROOT('Tree')