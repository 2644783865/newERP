<template>
  <div id="app" style="height:100%;">
    <div v-transfer-dom>
      <loading v-model="isLoading"></loading>
    </div>
    <div v-transfer-dom>
      <actionsheet :menus="menus" v-model="showMenu" @on-click-menu="logoutSystem"></actionsheet>
    </div>

    <view-box ref="viewBox" body-padding-top="46px" body-padding-bottom="55px">
      
      <x-header slot="header"
      style="width:100%;position:absolute;left:0;top:0;z-index:100;"
      :left-options="leftOptions"
      :right-options="rightOptions"
      :title="title"
      :transition="headerTransition"
      @on-click-more="onClickMore"></x-header>

      <transition :name="'vux-pop-' + (direction === 'forward' ? 'in' : 'out')">
        <router-view class="router-view"></router-view>
      </transition>

      <tabbar class="vux-home-tabbar" icon-class="vux-center" v-show="isTabbarPage" slot="bottom">
        <tabbar-item :link="{path:'/home'}" :selected="route.path === '/home'" badge="9">
          <span class="home-icon-22 vux-home-tabbar-icon-home" slot="icon" style="position:relative;top: -2px;">&#xe637;</span>
          <span slot="label">首页</span>
        </tabbar-item>
        <tabbar-item :link="{path:'/funlist'}" :selected="isFunctionPage">
          <span class="home-icon-22" slot="icon">&#xe633;</span>
          <span slot="label"><span v-if="componentName" class="vux-home-tabbar-component">{{componentName}}</span><span v-else>功能</span></span>
        </tabbar-item>
        <tabbar-item :link="{path:'/my'}" :selected="route.path === '/my'" show-dot>
          <span class="home-icon-22" slot="icon">&#xe630;</span>
          <span slot="label">我的</span>
        </tabbar-item>
      </tabbar>
    </view-box>
  </div>
</template>

<script>
import { Actionsheet, ButtonTab, ButtonTabItem, ViewBox, XHeader, Tabbar, TabbarItem, Loading, TransferDom } from 'vux'
import { mapState, mapActions } from 'vuex'
import * as types from './store/types'
export default {
  name: 'app',
  directives: {
    TransferDom
  },
  components: {
    ButtonTab,
    ButtonTabItem,
    ViewBox,
    XHeader,
    Tabbar,
    TabbarItem,
    Loading,
    Actionsheet
  },
  methods: {
    onClickMore () {
      this.showMenu = true
    },
    logoutSystem (user) {
      this.$store.commit(types.LOGOUT)
      this.$router.push({
        path: '/'
      })
    },
    ...mapActions([
      'updateHomePosition'
    ])
  },
  mounted () {
    this.handler = () => {
      if (this.path === '/funlist') {
        this.box = document.querySelector('#fun_list_box')
        this.updateHomePosition(this.box.scrollTop)
      }
    }
  },
  beforeDestroy () {
    this.box = document.querySelector('#fun_list_box')
    this.box.removeEventListener('scroll', this.handler, false)
  },
  watch: {
    path (path) {
      if (path === '/component/funlist') {
        this.$router.replace('/funlist')
        return
      }
      if (path === '/funlist') {
        setTimeout(() => {
          this.box = document.querySelector('#fun_list_box')
          if (this.box) {
            this.box.removeEventListener('scroll', this.handler, false)
            this.box.addEventListener('scroll', this.handler, false)
          }
        }, 1000)
      } else {
        this.box && this.box.removeEventListener('scroll', this.handler, false)
      }
    }
  },
  computed: {
    ...mapState({
      route: state => state.route,
      path: state => state.route.path,
      deviceready: state => state.app.deviceready,
      funTop: state => state.vux.funScrollTop,
      isLoading: state => state.vux.isLoading,
      direction: state => state.vux.direction,
      title: state => state.title,
      token: state => state.token
    }),
    isShowBar () {
      if (/home|my|pages/.test(this.path)) {
        return true
      }
      return false
    },
    leftOptions () {
      return {
        showBack: this.route.path !== '/'
      }
    },
    rightOptions () {
      return {
        showMore: true
      }
    },
    headerTransition () {
      return this.direction === 'forward' ? 'vux-header-fade-in-right' : 'vux-header-fade-in-left'
    },
    componentName () {
      if (this.route.path) {
        const parts = this.route.path.split('/')
        if (/pages/.test(this.route.path) && parts[2]) return parts[2]
      }
    },
    isFunctionPage () {
      return /pages|funlist/.test(this.route.path)
    },
    isTabbarPage () {
      return /home|funlist|my|pages/.test(this.route.path)
    },
    title () {
      if (this.route.path === '/') return '登录'
      if (this.route.path === '/home') return '首页'
      if (this.route.path === '/funlist') return '功能'
      if (this.route.path === '/my') return '我的'
      return this.componentName ? `首页/${this.componentName}` : '首页/~~'
    }
  },
  data () {
    return {
      showMenu: false,
      menus: {
        'language.noop': '<span class="menu-title">菜单</span>',
        'logout': '退出'
      }
    }
  }
}
</script>

<style lang="less">
@import '~vux/src/styles/reset.less';
@import '~vux/src/styles/1px.less';
@import '~vux/src/styles/tap.less';

#id {
  height:100%;
}
body {
  background-color: #fbf9fe;
}
html, body {
  height: 100%;
  width: 100%;
  overflow-x: hidden;
}
.home-icon-22 {
  font-family: 'vux-demo';
  font-size: 22px;
  color: #888;
}
.weui-tabbar.vux-home-tabbar {
  /** backdrop-filter: blur(10px);
  background-color: none;
  background: rgba(247, 247, 250, 0.5);**/
}
.vux-home-tabbar .weui-bar__item_on .home-icon-22 {
  color: #F70968;
}
.vux-home-tabbar .weui-tabbar_item.weui-bar__item_on .vux-demo-tabbar-icon-home {
  color: rgb(53, 73, 94);
}
.home-icon-22:before {
  content: attr(icon);
}
.vux-home-tabbar-component {
  background-color: #F70968;
  color: #fff;
  border-radius: 7px;
  padding: 0 4px;
  line-height: 14px;
}
.weui-tabbar__icon + .weui-tabbar__label {
  margin-top: 0!important;
}
.vux-home-header-box {
  z-index: 100;
  position: absolute;
  width: 100%;
  left: 0;
  top: 0;
}
@font-face {
  font-family: 'vux-demo';  /* project id 70323 */
  src: url('https://at.alicdn.com/t/font_h1fz4ogaj5cm1jor.eot');
  src: url('https://at.alicdn.com/t/font_h1fz4ogaj5cm1jor.eot?#iefix') format('embedded-opentype'),
  url('https://at.alicdn.com/t/font_h1fz4ogaj5cm1jor.woff') format('woff'),
  url('https://at.alicdn.com/t/font_h1fz4ogaj5cm1jor.ttf') format('truetype'),
  url('https://at.alicdn.com/t/font_h1fz4ogaj5cm1jor.svg#iconfont') format('svg');
}
.ib-icon {
  font-family: 'vux-demo';
  font-size: 20px;
  color: #04BE02;
}
.ib-icon-big {
  font-size: 28px;
}
.ib-icon:before {
  content: attr(icon);
}
.router-view {
  width: 100%;
  top: 46px;
}
.vux-pop-out-enter-active,
.vux-pop-out-leave-active,
.vux-pop-in-enter-active,
.vux-pop-in-leave-active {
  will-change: transform;
  transition: all 500ms;
  height: 100%;
  top: 46px;
  position: absolute;
  backface-visibility: hidden;
  perspective: 1000;
}
.vux-pop-out-enter {
  opacity: 0;
  transform: translate3d(-100%, 0, 0);
}
.vux-pop-out-leave-active {
  opacity: 0;
  transform: translate3d(100%, 0, 0);
}
.vux-pop-in-enter {
  opacity: 0;
  transform: translate3d(100%, 0, 0);
}
.vux-pop-in-leave-active {
  opacity: 0;
  transform: translate3d(-100%, 0, 0);
}
.menu-title {
  color: #888;
}
</style>
