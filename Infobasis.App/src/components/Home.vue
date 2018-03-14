<template>
  <div :style="{height: `${height}px`}">
    <divider>我的首页</divider>
	   <card :header="{title:'需要处理'}">
      <div slot="content" class="card-home-flex card-home-content01">
        <div class="vux-1px-l vux-1px-r">
          <span>1130</span>
          <br/>
          工程
        </div>
        <div class="vux-1px-r">
          <span>15</span>
          <br/>
          审批
        </div>
        <div class="vux-1px-r">
          <span>0</span>
          <br/>
          日历
        </div>
        <div>
          <span>88</span>
          <br/>
          七天处理
        </div>
      </div>
    </card>

    <br>
    <group title="公告">
      <cell title="公司端午节放假通知" value="更多" is-link></cell>
      <cell-box is-link>
        公司端午节放假通知 1
      </cell-box>
      <cell-box>
        公司端午节放假通知 2
      </cell-box>
      <cell title="公司端午节放假通知" value="更多" is-link></cell>
    </group>

    <br>
    <group title="业务提醒">
      <cell title="公司端午节放假通知" value="More" is-link></cell>
      <cell-box is-link>
        公司端午节放假通知 1
      </cell-box>
      <cell-box>
        公司端午节放假通知 2
      </cell-box>
      <cell title="公司端午节放假通知" value="More" is-link></cell>
    </group>
  </div>
</template>
<script>

import { Divider, Card, Group, Cell, CellBox } from 'vux'

export default {
  components: {
    Card,
    Divider,
    Group,
    Cell,
    CellBox
  },
  data () {
    return {
      height: window.innerHeight - 46 - 53
    }
  }
}

</script>
<style scoped lang="less">
@import '~vux/src/styles/1px.less';
.card-home-flex {
  display: flex;
}
.card-home-content01 {
  padding: 10px 0;
}
.card-padding {
  padding: 15px;
}
.card-home-flex > div {
  flex: 1;
  text-align: center;
  font-size: 12px;
}
.card-home-flex span {
  color: #f74c31;
}
</style>
