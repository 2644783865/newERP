<template>
  <div>
    <div class="infobasis-login">
      <img class="logo" src="../assets/lcyogo.gif">
      <h1> </h1>
    </div>
    <group title="登录创域ERP系统">
      <x-input v-model="companyCode" name="companyCode" placeholder="公司代号"></x-input>
      <x-input v-model="userName" name="userName" placeholder="用户名"></x-input>
      <x-input v-model="password" name="password" placeholder="密码" :min="3" :max="6" type="password"></x-input>
    </group>

    <group title="验证码" class="login-cells-vcodeform hide">
      <x-input title="验证码" class="login-cell-vcode" keyboard="number" is-type="china-mobile">
        <img slot="right" class="login-vcode-img" src="http://weui.github.io/weui/images/vcode.jpg">
      </x-input>
      <x-input class="login-vcode-btn">
        <x-button slot="right" type="primary" mini>发送验证码</x-button>
      </x-input>
    </group>

    <div style="padding:15px;">
      <x-button @click.native="onLoginClick" type="primary">登录</x-button>
    </div>
    <toast v-model="showLogin" type="text">{{loginWarnMsg}}</toast>
  </div>
</template>

<script>
import { Group, Cell, XInput, XButton, Toast, AjaxPlugin } from 'vux'
import * as types from '../store/types'
import axios from '../http.js'

export default {
  components: {
    Group,
    Cell,
    XInput,
    XButton,
    Toast,
    AjaxPlugin,
    axios
  },
  data () {
    return {
      // note: changing this line won't causes changes
      // with hot-reload because the reloaded component
      // preserves its current state and we are modifying
      // its initial state.
      msg: 'Hello World!',
      userName: 'sysadmin',
      companyCode: 'system',
      password: 'test',
      showLogin: false,
      loginWarnMsg: ''
    }
  },
  mounted () {
    this.$store.commit(types.TITLE, 'Login')
  },
  methods: {
    onLoginClick () {
      if (this.companyCode == null || this.companyCode.length === 0) {
        this.loginWarnMsg = '请输入公司代号'
        this.showLogin = true
        return
      }
      if (this.userName == null || this.userName.length === 0) {
        this.loginWarnMsg = '请输入用户名'
        this.showLogin = true
        return
      }
      if (this.password == null || this.password.length === 0) {
        this.loginWarnMsg = '请输入密码'
        this.showLogin = true
        return
      }

      let signinData = {CompanyCode: this.companyCode, UserName: this.userName, Password: this.password}
      let that = this
      axios.post('/users/signin', signinData).then(function (response) {
        var data = response.data
        localStorage.token = data.accessToken
        localStorage.tokenCreationDate = data.tokenCreationDate
        localStorage.userDisplayName = data.displayName
        // console.log(data.accessToken)
        that.loginWarnMsg = '登录成功'
        that.showLogin = true
        that.$store.commit(types.LOGIN, data.accessToken)
        // console.log(that.$store.state.vux.token)
        let redirect = decodeURIComponent(that.$route.query.redirect || '/home')
        // console.log(redirect)
        that.$router.push({
          path: redirect
        })
      }).catch(function (error) {
        console.log(error)
      })
      // this.$router.push('/home')
    }
  }
}
</script>

<style>
.infobasis-login {
  text-align: center;
}
.logo {
    text-align: left;
    margin-top: 10px;
}

.login-cell-vcode {
    float: left;
    width: 60%;
}
.login-vcode-btn {
    float: right;
    width: 25%;
}
.login-vcode-img {
    height: 30px;
}
.hide {
  display: none;
}
</style>
