<template>
  <div :style="{height: `${height}px`}">
    <divider>我的页面</divider>
    <group title="我的常用链接">
      <cell title="云盘" is-link></cell>
      <cell-box is-link>
        量房
      </cell-box>
      <cell-box>
        会议记录1
      </cell-box>
      <cell title="会议记录2" is-link></cell>
    </group>
    <div style="padding:15px;">
      <x-button type="warn" @click.native="logout">退出系统</x-button>
    </div>    
  </div>
</template>
<script>
import { Divider, Card, Group, Cell, CellBox, XButton } from 'vux'
import {mapState} from 'vuex'
import * as types from '../../store/types'

export default {
  components: {
    Card,
    Divider,
    Group,
    Cell,
    CellBox,
    XButton
  },
  data () {
    return {
      height: window.innerHeight - 46 - 53
    }
  },
  computed: mapState({
    title: state => state.title,
    token: state => state.token
  }),
  methods: {
    logout () {
      this.$store.commit(types.LOGOUT)
      this.$router.push({
        path: '/'
      })
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
