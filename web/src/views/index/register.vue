<template>
  <div class="container">

    <div class="tel-regist-page pc-style">
      <div class="regist-title">
        <span>Registration</span>
        <span @click="router.push({ name: 'login' })" class="toWxLogin">Login</span>
      </div>

      <div class="regist-padding">
        <div class="common-input">
          <img :src="MailIcon" class="left-icon">
          <div class="input-view">
            <input placeholder="e-mail" v-model="tData.loginForm.username" type="text" class="input">
            <p class="err-view">
            </p>
          </div>
        </div>
      </div>
      <div class="regist-padding">
        <div class="common-input">
          <img :src="PwdIcon" class="left-icon">
          <div class="input-view">
            <input placeholder="password" v-model="tData.loginForm.password" type="password" class="input">
            <p class="err-view">
            </p>
          </div>
        </div>
      </div>
      <div class="regist-padding">
        <div class="common-input">
          <img :src="PwdIcon" class="left-icon">
          <div class="input-view">
            <input placeholder="Please enter your password again" v-model="tData.loginForm.repassword" type="password"
              class="input">
            <p class="err-view">
            </p>
          </div>
        </div>
      </div>
      <div class="regist-padding">
        <div class="common-input">
          <img :src="CountryIcon" class="left-icon">
          <div class="input-view">
            <select v-model="tData.loginForm.country" class="input">
              <option value="USA" default>USA</option>
              <option value="Canada">Canada</option>
              <option value="UK">UK</option>
              <option value="China">China</option>
              <option value="Japan">Japan</option>
            </select>
            <p class="err-view">
            </p>
          </div>
        </div>
      </div>
      <div class="regist-padding">
        <div class="common-input">
          <img :src="TelIcon" class="left-icon">
          <div class="input-view">
            <!-- <input placeholder="" 
            v-model="tData.loginForm.phoneNumber" type="text" class="input"> -->
            <TelPhone v-model:phonePrefix="tData.loginForm.phonePrefix"
              v-model:phoneNumber="tData.loginForm.phoneNumber" />
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
import { message } from "ant-design-vue";
import MailIcon from '/@/assets/images/mail-icon.svg';
import PwdIcon from '/@/assets/images/pwd-icon.svg';
import TelIcon from '/@/assets/images/tel-icon.svg';
import CountryIcon from '/@/assets/images/location.svg';
import TelPhone from "./telphone.vue";
import { watch, ref } from "vue";
const router = useRouter();

const tData = reactive({
  loginForm: {
    username: '',
    password: '',
    repassword: '',
    country: ref('USA'),
    phoneNumber: '',
    country_codes: {
      "China": "86",
      "USA": "+1",
      "UK": "+44",
      "Japan": "+81",
      "Canada": "+1",
    },
    postalCode: '',
    phonePrefix: ref('+1'),
    creDitCard: '',
    lastName: '',
    firstName: '',
    cardType: {
      "MasterCard": ["51", "52", "53", "54", "55"],
      "Visa": ["4"],
      "American Express": ["34", "37"]
    },
    expirationDate: ''
  }
})

watch(() => tData.loginForm.country, (newCountry) => {
  tData.loginForm.phonePrefix = tData.loginForm.country_codes[newCountry];
});
const handleRegister = () => {
  console.log('login')
  if (tData.loginForm.username === ''
    || tData.loginForm.password === ''
    || tData.loginForm.repassword === '') {
    message.warn('Not null!')
    return;
  }

  const specialCharacters = /[;:!@#$%^*+?\/<>1234567890]/g;
  const username = tData.loginForm.username
  if (username.match(specialCharacters)) {
    message.warn('Invalid  characters!')
  }

  const emailRegex =/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

  if(!emailRegex.test(username)){
    message.warn('Invalid  Email!')
  }

  const phoneRegex = /^(\(\d{3}\)|\d{3})\d{3}-\d{4}$/; // 添加电话号码正则表达式
  const phoneNumber = tData.loginForm.phoneNumber; // 获取电话号码
  const phonePrefix = tData.loginForm.phonePrefix; // 获取电话号码
  const county = tData.loginForm.country;
  const postalCode =tData.loginForm.postalCode;
  if (county == 'USA' || county == 'Canada') {
    //验证美国或加拿大手机号码，其他国家请自行验证
    if (!phoneNumber || !phoneRegex.test(phoneNumber)) { // 验证电话号码格式
      message.warn('Invalid phone number format!');
      return;
    }
    if()
  }


  userRegisterApi({
    username: tData.loginForm.username,
    password: tData.loginForm.password,
    rePassword: tData.loginForm.repassword,
    phoneNumber: tData.loginForm.phoneNumber, // 添加电话号码到请求参数中
    countryCode: phonePrefix + "-" + phoneNumber // 添加国家代码到请求参数中
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
  width: 400px;
  height: 464px;
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
