import Vue from 'vue'
import Router from 'vue-router'
import HelloFromVux from '@/components/HelloFromVux'
import Login from '@/components/Login'
import Home from '@/components/Home'
import FunctionPage from '@/components/FunctionPage'
import My from '@/components/pages/My'
import * as types from '../store/types'
import store from '../store/store'

Vue.use(Router)
// console.log(store)
// 页面刷新时，重新赋值token
if (window.localStorage.getItem('token')) {
  store.commit(types.LOGIN, window.localStorage.getItem('token'))
}

export {
}

const routes = [
  {
    path: '/',
    name: 'Login',
    component: Login
  },
  {
    path: '/hello',
    name: 'Hello',
    component: HelloFromVux
  },
  {
    path: '/home',
    name: 'Home',
    component: Home,
    meta: {
      requireAuth: true
    }
  },
  {
    path: '/funlist',
    name: 'FunctionPage',
    component: FunctionPage,
    meta: {
      requireAuth: true
    }
  },
  {
    path: '/my',
    name: 'My',
    component: My,
    meta: {
      requireAuth: true
    }
  },
  {
    path: '*',
    redirect: '/'
  }
]

const router = new Router({
  routes
})

router.beforeEach((to, from, next) => {
  if (to.matched.some(r => r.meta.requireAuth)) {
    if (store.state.vux.token) {
      next()
    } else {
      next({
        path: '/',
        query: {redirect: to.fullPath}
      })
    }
  } else {
    next()
  }
})

export default router
