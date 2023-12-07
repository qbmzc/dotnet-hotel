<template>
  <div class="container">

    <div class="tel-regist-page pc-style">
      <div class="regist-title">
        <span>Registration</span>
        <span @click="router.push({ name: 'login' })" class="toWxLogin">Login</span>
      </div>
      <div class="regist-padding">
        <div class="common-input">
          <img :src="UserIcon" class="left-icon">
          <div class="input-view">
            <input placeholder="username" v-model="tData.loginForm.username" type="text" required="true" class="input">
            <p class="err-view">
            </p>
          </div>
        </div>
      </div>
      <div class="regist-padding">
        <div class="common-input">
          <img :src="PwdIcon" class="left-icon">
          <div class="input-view">
            <input placeholder="password" v-model="tData.loginForm.password" type="password" required="true" class="input" />
            <p class="err-view">
            </p>
          </div>
        </div>
      </div>
      <div class="regist-padding">
        <div class="common-input">
          <img :src="PwdIcon" class="left-icon">
          <div class="input-view">
            <input placeholder="Please enter your password again" v-model="tData.loginForm.repassword" required="true" type="password"
              class="input" />
            <p class="err-view">
            </p>
          </div>
        </div>
      </div>
     
      <div class="tel-login">
        <div class="next-btn-view">
          <button class="next-btn" @click="handleRegister">Regist</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { userRegisterApi } from '/@/api/user'
import { DatePicker, message } from "ant-design-vue";
import MailIcon from '/@/assets/images/mail-icon.svg';
import UserIcon from '/@/assets/images/user.svg';
import PwdIcon from '/@/assets/images/pwd-icon.svg';
import TelIcon from '/@/assets/images/tel-icon.svg';
import CountryIcon from '/@/assets/images/location.svg';
import CreditCardIcon from '/@/assets/images/credit-card.svg';
import TelPhone from "./telphone.vue";
import { watch, ref } from "vue";
const router = useRouter();

const tData = reactive({
  loginForm: {
    username: '',
    password: '',
    repassword: '',
  }
})


const handleRegister = () => {
  console.log('login')
  console.log(tData.loginForm)
  if (tData.loginForm.username === ''
    || tData.loginForm.password === ''
    || tData.loginForm.repassword === '') {
    message.warn('Not null!')
    return;
  }


  const specialCharacters = /[;:!@#$%^*+?\/<>1234567890]/g;
  const username = tData.loginForm.username;
  
  if ( specialCharacters.test(username)) {
    message.warn('Username contains Invalid  characters!')
  }





  userRegisterApi({
    username: tData.loginForm.username,
    password: tData.loginForm.password,
    rePassword: tData.loginForm.repassword
  }).then(res => {
    message.success('success!')
    router.push({ name: 'login' })
  }).catch(err => {
    message.error(err.msg || 'failed')
  })
}


</script>

<style scoped lang="less">
div {
  display: block;
}

*,
:after,
:before,
img {
  border-style: none;
}

*,
:after,
:before {
  border-width: 0;
  border-color: #dae1e7;
}

.container {
  max-width: 100%;
  //background: #142131;
  background-image: url('../images/admin-login-bg.jpg');
  background-size: cover;
  object-fit: cover;
  height: 100vh;
  overflow: hidden;
  display: flex;
  justify-content: center;
  align-items: center;
}

.pc-style {
  position: relative;
  width: 600px;
  height: 90vh;
  background: #fff;
  -webkit-box-shadow: 2px 2px 6px #aaa;
  box-shadow: 2px 2px 6px #aaa;
  border-radius: 4px;
}

.tel-regist-page {
  overflow: hidden;

  .regist-title {
    font-size: 14px;
    color: #1e1e1e;
    font-weight: 500;
    height: 24px;
    line-height: 24px;
    margin: 40px 0;
    padding: 0 28px;

    .toWxLogin {
      color: #3d5b96;
      float: right;
      cursor: pointer;
    }
  }

  .regist-padding {
    padding: 0 28px;
    margin-bottom: 8px;
  }
}

.common-input {
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-align: start;
  -ms-flex-align: start;
  align-items: flex-start;

  .left-icon {
    margin-right: 12px;
    width: 24px;
  }

  .input-view {
    -webkit-box-flex: 1;
    -ms-flex: 1;
    flex: 1;

    .input {
      font-weight: 500;
      font-size: 14px;
      color: #1e1e1e;
      height: 26px;
      line-height: 26px;
      padding: 0;
      display: block;
      width: 100%;
      letter-spacing: 1.5px;
      outline: none; // 去掉边框线
    }

    .err-view {
      margin-top: 4px;
      height: 16px;
      line-height: 16px;
      font-size: 12px;
      color: #f62a2a;
    }
  }
}

.tel-login {
  padding: 0 28px;
}

.next-btn {
  background: #3d5b96;
  border-radius: 4px;
  color: #fff;
  font-size: 14px;
  font-weight: 500;
  height: 40px;
  line-height: 40px;
  text-align: center;
  width: 100%;
  outline: none;
  cursor: pointer;
}
</style>
