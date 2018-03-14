import Vue from 'vue'
import FastClick from 'fastclick'
import VueRouter from 'vue-router'
import { sync } from 'vuex-router-sync'
import App from './App'
// import Login from './components/Login'
// import Home from './components/Home'
// import FunctionPage from '@/components/FunctionPage'
// import My from '@/components/pages/My'
import Vuex from 'vuex'
import ibconfig from '../config/ibconfig.js'
import store from './store/store'
// import auth from './jwt'
import axios from './http.js'
import router from './router'

Vue.use(store)
Vue.use(Vuex)
Vue.use(VueRouter)
Vue.use(ibconfig)
Vue.use(router)
// Vue.use(axios)

Vue.prototype.axios = axios

// console.log(Vue.http)
// console.log(this.$http)
// Vue.http.headers.common['Authorization'] = 'Bearer ' + localStorage.getItem('id_token')
// auth.checkAuth()

require('es6-promise').polyfill()

FastClick.attach(document.body)

Vue.config.productionTip = false

// plugins
import { LocalePlugin, DevicePlugin, ToastPlugin, AlertPlugin, ConfirmPlugin, LoadingPlugin, WechatPlugin, AjaxPlugin, AppPlugin } from 'vux'
Vue.use(DevicePlugin)
Vue.use(ToastPlugin)
Vue.use(AlertPlugin)
Vue.use(ConfirmPlugin)
Vue.use(LoadingPlugin)
Vue.use(WechatPlugin)
Vue.use(AjaxPlugin)
Vue.use(LocalePlugin)

// test
if (process.env.platform === 'app') {
  Vue.use(AppPlugin, store)
}

sync(store, router)

// simple history management
const history = window.sessionStorage
history.clear()
let historyCount = history.getItem('count') * 1 || 0
history.setItem('/', 0)

router.beforeEach(function (to, from, next) {
  store.commit('updateLoadingStatus', {isLoading: true})

  const toIndex = history.getItem(to.path)
  const fromIndex = history.getItem(from.path)

  if (toIndex) {
    if (!fromIndex || parseInt(toIndex, 10) > parseInt(fromIndex, 10) || (toIndex === '0' && fromIndex === '0')) {
      store.commit('updateDirection', {direction: 'forward'})
    } else {
      store.commit('updateDirection', {direction: 'reverse'})
    }
  } else {
    ++historyCount
    history.setItem('count', historyCount)
    to.path !== '/' && history.setItem(to.path, historyCount)
    store.commit('updateDirection', {direction: 'forward'})
  }

  if (/\/http/.test(to.path)) {
    let url = to.path.split('http')[1]
    window.location.href = `http${url}`
  } else {
    next()
  }
})

router.afterEach(function (to) {
  store.commit('updateLoadingStatus', {isLoading: false})
})

export default store

/* eslint-disable no-new */
new Vue({
  store,
  router,
  axios,
  render: h => h(App)
}).$mount('#app-box')
