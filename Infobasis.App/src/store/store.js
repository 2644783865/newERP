import Vuex from 'vuex'
import Vue from 'vue'

Vue.use(Vuex)
let store = new Vuex.Store({
  modules: {
  }
})

store.registerModule('vux', {
  state: {
    homeScrollTop: 0,
    isLoading: false,
    direction: 'forward',
    token: null,
    user: {},
    title: ''
  },
  mutations: {
    updateHomePosition (state, payload) {
      state.homeScrollTop = payload.top
    },
    updateLoadingStatus (state, payload) {
      state.isLoading = payload.isLoading
    },
    updateDirection (state, payload) {
      state.direction = payload.direction
    },
    login (state, data) {
      localStorage.token = data
      state.token = data
    },
    logout (state, data) {
      localStorage.removeItem('token')
      state.token = null
    },
    title (state, data) {
      state.title = data
    }
  },
  actions: {
    updateHomePosition ({commit}, top) {
      commit({type: 'updateHomePosition', top: top})
    }
  }
})

export default store
