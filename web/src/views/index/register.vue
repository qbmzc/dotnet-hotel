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
            <input placeholder="FirstName" v-model="tData.loginForm.firstName" type="text" required="true" class="input">
            <p class="err-view">
            </p>
          </div>
        </div>
      </div>
      <div class="regist-padding">
        <div class="common-input">
          <img :src="UserIcon" class="left-icon">
          <div class="input-view">
            <input placeholder="LastName" v-model="tData.loginForm.lastName" type="text" required="true" class="input">
            <p class="err-view">
            </p>
          </div>
        </div>
      </div>
      <div class="regist-padding">
        <div class="common-input">
          <img :src="MailIcon" class="left-icon">
          <div class="input-view">
            <input placeholder="e-mail" v-model="tData.loginForm.email" type="email" required="true" class="input">
            <p class="err-view">
            </p>
          </div>
        </div>
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
            <TelPhone v-model:phonePrefix="tData.loginForm.phonePrefix"
             v-model:mobile="tData.loginForm.mobile"></TelPhone>
            <p class="err-view">
            </p>
          </div>
        </div>
      </div>
      <div class="regist-padding">
        <div class="common-input">
          <img :src="CountryIcon" class="left-icon">
          <div class="input-view">
            <input placeholder="City" v-model="tData.loginForm.city" required="true" type="text" class="input">
            <p class="err-view">
            </p>
          </div>
        </div>
      </div>
      <div class="regist-padding">
        <div class="common-input">
          <img :src="CountryIcon" class="left-icon">
          <div class="input-view">
            <input placeholder="State" v-model="tData.loginForm.state" required="true" type="text" class="input">
            <p class="err-view">
            </p>
          </div>
        </div>
      </div>
      <!-- postal code -->
      <div class="regist-padding">
        <div class="common-input">
          <img :src="TelIcon" class="left-icon">
          <div class="input-view">
            <input placeholder="PostalCode" v-model="tData.loginForm.postalCode" required="true" type="text" class="input">
            <p class="err-view">
            </p>
          </div>
        </div>
      </div>
      <div class="regist-padding">
        <div class="common-input">
          <img :src="CreditCardIcon" class="left-icon">
          <div class="input-view">
            <select v-model="tData.loginForm.cardType" class="input">
              <option value="MasterCard" default>MasterCard</option>
              <option Visa="Visa">Visa</option>
              <option value="American Express">American Express</option>
            </select>
            <p class="err-view">
            </p>
          </div>
        </div>
      </div>
      <div class="regist-padding">
        <div class="common-input">
          <img :src="CreditCardIcon" class="left-icon">
          <div class="input-view">
            <input placeholder="creditCard" v-model="tData.loginForm.creditCard" type="number" required="true" minlength="15" maxlength="16" class="input">
            <p class="err-view">
            </p>
          </div>
        </div>
      </div>
      <div class="regist-padding">
        <div class="common-input">
          <img :src="CreditCardIcon" class="left-icon">
          <div class="input-view">
            <DatePicker placeholder="Expiration Date" format="MM/YYYY" v-model:value="tData.loginForm.expirationDate"></DatePicker>
            <p class="err-view">
            </p>
          </div>
        </div>
      </div>
      <div class="regist-padding">
        <div class="common-input">
          <img :src="CreditCardIcon" class="left-icon">
          <div class="input-view">
            <input placeholder="holder's name" v-model="tData.loginForm.holder" type="text" required="true" class="input">
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
    country: ref('USA'),
    city: '',
    state: '',
    mobile: '',
    email: '',
    holder: '',
    country_codes: {
      "China": "+86",
      "USA": "+1",
      "UK": "+44",
      "Japan": "+81",
      "Canada": "+1",
    },
    postalCode: '',
    phonePrefix: ref('+1'),
    creditCard: '',
    lastName: '',
    firstName: '',
    cardType: ref('Visa'),
    cardTypes: {
      "MasterCard": ["51", "52", "53", "54", "55"],
      "Visa": ["4"],
      "American Express": ["34", "37"]
    },
    expirationDate: '',
    specialCharacters: /[;:!@#$%^*+?\/<>1234567890]/g,
    USAPostalCodeRegex: /^(\d{5})(-|\s)?\d{4}$/,
    CanadaPostalCodeRegex: /^[A-VXY][0-9][A-Z]{2} ?[0-9][A-Z]{2}$/,
    phoneRegex: /^(\(\d{3}\)|\d{3})\d{3}-\d{4}$/,
  }
})

watch(() => tData.loginForm.country, (newCountry) => {
  tData.loginForm.phonePrefix = tData.loginForm.country_codes[newCountry];
});
const handleRegister = () => {
  console.log('login')
  console.log(tData.loginForm)
  if (tData.loginForm.username === ''
    || tData.loginForm.password === ''
    || tData.loginForm.repassword === ''
    || tData.loginForm.email === ''
    || tData.loginForm.firstName === ''
    || tData.loginForm.lastName === ''
    || tData.loginForm.country === ''
    || tData.loginForm.city === ''
    || tData.loginForm.state === '' 
    || tData.loginForm.mobile === ''
    || tData.loginForm.cardType === ''
    || tData.loginForm.creditCard === ''
    || tData.loginForm.expirationDate === ''
    || tData.loginForm.holder === '') {
    message.warn('Not null!')
    return;
  }


  const specialCharacters = /[;:!@#$%^*+?\/<>1234567890]/g;
  const firstName = tData.loginForm.firstName;
  const lastName = tData.loginForm.lastName;
  const username = tData.loginForm.username;
  const country = tData.loginForm.country;
  const holder = tData.loginForm.holder;
  if (specialCharacters.test(firstName)
    || specialCharacters.test(lastName)
    || specialCharacters.test(username)
    || specialCharacters.test(country)
    || specialCharacters.test(holder)) {
    message.warn('Invalid  characters!')
  }

  const postalCodeRegex = tData.loginForm.country === 'USA'
    ? /^(\d{5})(-|\s)?\d{4}$/
    : /^[A-VXY][0-9][A-Z]{2} ?[0-9][A-Z]{2}$/;



  const phoneRegex = /^(\(\d{3}\)|\d{3})\d{3}-\d{4}$/; // 添加电话号码正则表达式
  const mobile = tData.loginForm.mobile; // 获取电话号码
  const phonePrefix = tData.loginForm.phonePrefix; // 获取电话号码
  const county = tData.loginForm.country;
  const postalCode = tData.loginForm.postalCode;
  if (county == 'USA' || county == 'Canada') {
    //验证美国或加拿大手机号码，其他国家请自行验证
    if (!mobile || !phoneRegex.test(mobile)) { // 验证电话号码格式
      message.warn('Invalid phone number format!');
      return;
    }
  }
  if (postalCodeRegex.test(postalCode)) {
    message.warn('Invalid  postalCode!')
  }
  const creditCard = tData.loginForm.creditCard;
  const cardType = tData.loginForm.cardType;
  if (cardType == "American Express") {
    if (creditCard.length != 15 || !creditCard.startsWith("34") || !creditCard.startsWith("37")) {
      message.warn("Invalid creditCard")
    }
  }
  if (cardType == "Aisa") {
    if (creditCard.length != 16 || !creditCard.startsWith("4")) {
      message.warn("Invalid creditCard")
    }
  }
  if (cardType == "MasterCard") {
    if (creditCard.length != 16 || !creditCard.startsWith("51")
      || !creditCard.startsWith("52")
      || !creditCard.startsWith("53")
      || !creditCard.startsWith("54")
      || !creditCard.startsWith("55")) {
      message.warn("Invalid creditCard")
    }
  }


  userRegisterApi({
    username: tData.loginForm.username,
    password: tData.loginForm.password,
    rePassword: tData.loginForm.repassword,
    mobile: phonePrefix + "-" + tData.loginForm.mobile, // 添加国家代码到请求参数中
    firstName: tData.loginForm.firstName,
    lastName: tData.loginForm.lastName,
    email: tData.loginForm.email,
    country: tData.loginForm.country,
    city: tData.loginForm.city,
    state: tData.loginForm.state,
    postalCode: tData.loginForm.postalCode,
    cardType: tData.loginForm.cardType,
    creditCard: tData.loginForm.creditCard,
    expirationDate: tData.loginForm.expirationDate,
    holder: tData.loginForm.holder,
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
