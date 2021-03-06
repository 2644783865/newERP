--select * from SYtbCompany

INSERT INTO SYtbCompany
(
	Name,
	CompanyLogo,
	FullName,
	IndustryID,
	MaxEmployees,
	StrongPasswords,
	MinPasswordLength,
	MaxPasswordLength,
	MaxLogonAttempts,
	AccountLockoutMinutes,
	MustAcceptLegal,
	CompanyStatus,
	MaxUsers,
	CompanyStatusDesc,
	CompanyCode,
	IsSystemAdminCompany,
	ClientAdminAccount
)
SELECT
	Name = N'系统管理',
	CompanyLogo = NULL,
	FullName = N'系统管理',
	IndustryID = 0,
	MaxEmployees = 0,
	StrongPasswords = 1,
	MinPasswordLength = 6,
	MaxPasswordLength = 8,
	MaxLogonAttempts = 10,
	AccountLockoutMinutes = NULL,
	MustAcceptLegal = 0,
	CompanyStatus = 0,
	MaxUsers = 200,
	CompanyStatusDesc = N'正常',
	CompanyCode = 'system',
	IsSystemAdminCompany = 1,
	ClientAdminAccount = 'sysadmin'
WHERE NOT EXISTS(SELECT 1 FROM SYtbCompany WHERE CompanyCode = 'system')

--9dpSwD5rvrzapRI31B5BcownHYfmWZVI
INSERT INTO SYtbUser
(
	Name,
	Email,
	[Password],
	LogonCount,
	FailedLogonCount,
	MustChangePassword,
	Enabled,
	DefaultPageSize,
	IsClientAdmin,
	CompanyID,
	CreateDatetime,
	ChineseName,
	EnglishName,
	UserType
)
SELECT
	Name = 'sysadmin',
	Email = NULL,
	[Password] = '9dpSwD5rvrzapRI31B5BcownHYfmWZVI',
	LogonCount = 0,
	FailedLogonCount = 0,
	MustChangePassword = 0,
	Enabled = 1,
	DefaultPageSize = 30,
	IsClientAdmin = 0,
	CompanyID = SYtbCompany.ID,
	CreateDatetime = GETDATE(),
	ChineseName = N'系统管理员',
	EnglishName = N'System Admin',
	UserType = 0 --employee
FROM  SYtbCompany
WHERE SYtbCompany.CompanyCode = 'system'
	AND NOT EXISTS(SELECT 1 FROM SYtbUser WHERE Name = 'sysadmin' AND CompanyID = SYtbCompany.ID)

DECLARE @CompanyID int,
	@UserID int

SELECT @CompanyID = ID
FROM SYtbCompany
WHERE SYtbCompany.CompanyCode = 'system'

SELECT @UserID = ID
FROM SYtbUser
WHERE Name = 'sysadmin' AND CompanyID = @CompanyID

INSERT INTO SYtbSystemAdmin
(
	UserID,
	Active,
	CreateDatetime
)
SELECT
	UserID = SYtbUser.ID,
	Active = 1,
	CreateDatetime = GETDATE()
FROM SYtbUser
INNER JOIN SYtbCompany
ON SYtbUser.CompanyID = SYtbCompany.ID
WHERE SYtbCompany.CompanyCode = 'system'
	AND NOT EXISTS(SELECT 1 FROM SYtbSystemAdmin WHERE UserID = SYtbUser.ID)

INSERT INTO SYtbPermissionRole
(
	Name,
	Code,
	DisplayOrder,
	IsActive,
	ForbidDelete,
	CompanyID,
	CreateDatetime,
	IsClientAdminRole
)
SELECT
	Name = N'管理员',
	Code = 'Admin',
	DisplayOrder = 1,
	IsActive = 1,
	ForbidDelete = 1,
	CompanyID = SYtbCompany.ID,
	CreateDatetime = GETDATE(),
	IsClientAdminRole = 1
FROM SYtbUser
INNER JOIN SYtbCompany
ON SYtbUser.CompanyID = SYtbCompany.ID
AND SYtbCompany.CompanyCode = 'system'
WHERE NOT EXISTS(
	SELECT 1 FROM SYtbPermissionRole
	WHERE SYtbPermissionRole.CompanyID = SYtbCompany.ID
		AND SYtbPermissionRole.Code = 'Admin'
	)

--sp_help SYtbUserPermissionRole

INSERT INTO SYtbUserPermissionRole
(
	UserID,
	PermissionRoleID,
	CompanyID,
	CreateByID,
	CreateByName,
	CreateDatetime
)
SELECT
	UserID = SYtbUser.ID,
	PermissionRoleID = SYtbPermissionRole.ID,
	CompanyID = SYtbCompany.ID,
	CreateByID = 0,
	CreateByName = 'System',
	CreateDatetime = GETDATE()
FROM SYtbUser
INNER JOIN SYtbCompany
ON SYtbUser.CompanyID = SYtbCompany.ID
AND SYtbCompany.CompanyCode = 'system'
INNER JOIN SYtbPermissionRole
ON SYtbPermissionRole.CompanyID = SYtbCompany.ID
AND SYtbPermissionRole.Code = 'Admin'
AND NOT EXISTS(SELECT 1 FROM SYtbUserPermissionRole
				WHERE SYtbUserPermissionRole.CompanyID = SYtbCompany.ID
					AND SYtbUserPermissionRole.PermissionRoleID = SYtbPermissionRole.ID)

EXEC usp_SY_CreateNewComanyDefaultData @CompanyID, @UserID

--select * from SYtbUserPermissionRole