<template>
  <div class="content-list">
    <div class="list-title">SetUp</div>
    <a-spin :spinning="loading" style="min-height: 200px;">
      <div class="list-content">
        <div class="edit-view" :model="tData.form" :rules="tData.rules">
          <div class="item flex-view">
            <div class="label">Avatar</div>
            <div class="right-box avatar-box flex-view">
              <img v-if="tData.form && tData.form.avatar" :src="'data:image/jpeg;base64,' + tData.form.avatar"
                class="avatar">
              <img v-else :src="AvatarIcon" class="avatar">
              <div class="change-tips flex-view">
                <a-upload name="file" accept="image/*" :multiple="false" :before-upload="beforeUpload">
                  <label>click change profile picture</label>
                </a-upload>
                <p class="tip">the image format supports gif png jpeg and the size is not less than 200 px and less than 4
                  mb</p>
              </div>
            </div>
          </div>
          <div class="item flex-view">
            <div class="label">Nickname</div>
            <div class="right-box">
              <input type="text" v-model="tData.form.nickname" :rules="tData.rules.nickname" placeholder="Nickname"
                maxlength="20" class="input-dom">
              <p class="tip">it cannot exceed 20 characters in length</p>
            </div>
          </div>

          <div class="item flex-view">
            <div class="label">E-Mail</div>
            <div class="right-box">
              <input type="email" v-model="tData.form.email" :rules="tData.rules.email" placeholder="email"
                maxlength="100" class="input-dom web-input">
            </div>
          </div>
          <div class="item flex-view">
            <div class="label">FirstName</div>
            <div class="right-box">
              <input type="text" v-model="tData.form.firstName" :rules="tData.rules.fileName" placeholder="FirstName"
                maxlength="100" class="input-dom web-input">
            </div>
          </div>
          <div class="item flex-view">
            <div class="label">LastName</div>
            <div class="right-box">
              <input type="text" v-model="tData.form.lastName" :rules="tData.rules.lastName" placeholder="LastName"
                maxlength="100" class="input-dom web-input">
            </div>
          </div>
          <div class="item flex-view">
            <div class="label">Country</div>
            <div class="right-box">
              <select v-model="tData.form.country" class="input">
                <option value="USA" default>USA</option>
                <option value="Canada">Canada</option>
                <option value="UK">UK</option>
                <option value="China">China</option>
                <option value="Japan">Japan</option>
              </select>
            </div>
          </div>
          <div class="item flex-view">
            <div class="label">Mobile</div>
            <div class="right-box">
              <TelPhone v-model:phonePrefix="tData.form.phonePrefix" v-model:mobile="tData.form.mobile"></TelPhone>
            </div>
          </div>
          <div class="item flex-view">
            <div class="label">City</div>
            <div class="right-box">
              <input type="text" v-model="tData.form.city" placeholder="City" maxlength="100" class="input-dom web-input">
            </div>
          </div>
          <div class="item flex-view">
            <div class="label">State</div>
            <div class="right-box">
              <input type="text" v-model="tData.form.state" placeholder="state" maxlength="100"
                class="input-dom web-input">
            </div>
          </div>
          <div class="item flex-view">
            <div class="label">PostalCode</div>
            <div class="right-box">
              <input type="text" v-model="tData.form.postalCode" placeholder="postalCode" maxlength="100"
                class="input-dom web-input">
            </div>
          </div>
          <div class="item flex-view">
            <div class="label">CardType</div>
            <div class="right-box">
              <select v-model="tData.form.cardType" class="input">
                <option value="MasterCard" default>MasterCard</option>
                <option Visa="Visa">Visa</option>
                <option value="American Express">American Express</option>
              </select>
            </div>
          </div>
          <div class="item flex-view">
            <div class="label">CreditCard</div>
            <div class="right-box">
              <input type="number" v-model="tData.form.creditCard" :rules="tData.rules.creditCard"
                placeholder="creditCard" maxlength="16" minlength="15" class="input-dom web-input">
            </div>
          </div>
          <div class="item flex-view">
            <div class="label">ExpirationDate</div>
            <div class="right-box">
              <DatePicker placeholder="Expiration Date" format="MM/YYYY" v-model:value="tData.form.expirationDate">
              </DatePicker>
            </div>
          </div>
          <div class="item flex-view">
            <div class="label">holder's Name</div>
            <div class="right-box">
              <input placeholder="holder's name" v-model="tData.form.holder" :rules="tData.rules.holder" type="text"
                required="true" class="input-dom web-input">
            </div>
          </div>
          <div class="item flex-view">
            <div class="label">Description</div>
            <div class="right-box">
              <textarea v-model="tData.form.description" placeholder="description" maxlength="200" class="intro">
          </textarea>
              <p class="tip">it cannot exceed 200 characters in length</p>
            </div>
          </div>
          <button class="save mg" @click="submit()">Save</button>
        </div>
      </div>
    </a-spin>
  </div>
</template>

<script setup>
import { DatePicker, message } from "ant-design-vue";
import { detailApi, updateUserInfoApi } from '/@/api/user'
import { BASE_URL } from "/@/store/constants";
import { useUserStore } from "/@/store";
import AvatarIcon from '/@/assets/images/avatar.jpg'
import TelPhone from "./telphone.vue";
import { watch, ref } from "vue";
import dayjs from "dayjs";
const country_codes = {
  "China": "+86",
  "USA": "+1",
  "UK": "+44",
  "Japan": "+81",
  "Canada": "+1",
  "string": "+0"
};
const router = useRouter();
const userStore = useUserStore();
let loading = ref(false)
let tData = reactive({
  form: {
    avatar: undefined,
    avatarFile: undefined,
    nickname: undefined,
    email: undefined,
    mobile: undefined,
    description: undefined,
    country: ref('USA'),
    city: '',
    state: '',
    mobile: '',
    email: '',
    holder: '',

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
  },
  rules: {
    nickname: [{ required: true, message: 'please enter', trigger: 'change' }, {
      minlength: 3, maxlength: 20, message: '', trigger: 'blur'
    }],
    email: [{ required: true, message: 'please enter', trigger: 'change' }],
    mobile: [{ required: true, message: 'please select', trigger: 'change' }],
    description: [{ required: true, message: 'please select', trigger: 'change' }],
  },
})

onMounted(() => {
  getUserInfo()
})

watch(() => tData.form.country, (newCountry) => {
  console.log('newCountry' + newCountry)
  if (newCountry == 'string' || newCountry == null) {
    newCountry = 'USA'
  } else {
    tData.form.phonePrefix = country_codes[newCountry];
  }
});

const beforeUpload = (file) => {
  // 改文件名
  const fileName = new Date().getTime().toString() + '.' + file.type.substring(6)
  const copyFile = new File([file], fileName)
  console.log(copyFile)
  tData.form.avatarFile = copyFile
  return false
}

const getUserInfo = () => {
  loading.value = true
  let userId = userStore.user_id
  detailApi({ userId: userId }).then(res => {
    tData.form = res.data
    tData.form.expirationDate = dayjs(res.data.expirationDate, "MM/YYYY")
    if (tData.form.avatar) {
      // tData.form.avatar = BASE_URL + '/api/staticfiles/avatar/' + tData.form.avatar
      tData.form.avatar = tData.form.avatar
    }
    loading.value = false
  }).catch(err => {
    console.log(err)
    loading.value = false
  })
}

const beforeSubmit = () => {


 


}
const submit = () => {
  console.log(tData.form)
  if (tData.form.email === ''
    || tData.form.firstName === ''
    || tData.form.postalCode === ''
    || tData.form.nickname === ''
    || tData.form.lastName === ''
    || tData.form.country === ''
    || tData.form.city === ''
    || tData.form.state === ''
    || tData.form.mobile === ''
    || tData.form.cardType === ''
    || tData.form.creditCard === ''
    || tData.form.expirationDate === ''
    || tData.form.holder === '') {
    message.warn('Please fill in all the fields')
    return;
  }

  const specialCharacters = /[;:!@#$%^*+?\/<>1234567890]/g;
  const firstName = tData.form.firstName;
  const lastName = tData.form.lastName;
  const country = tData.form.country;
  const holder = tData.form.holder;
  if (specialCharacters.test(firstName)
    || specialCharacters.test(lastName)
    || specialCharacters.test(country)
    || specialCharacters.test(holder)) {
    message.warn('Invalid  characters!')
  }

  const postalCodeRegex = tData.form.country === 'USA'
    ? /^(\d{5})(-|\s)?\d{4}$/
    : /^[A-VXY][0-9][A-Z]{2} ?[0-9][A-Z]{2}$/;



  const phoneRegex = /^\(?(\d{3})\)?[- ]?(\d{3})[- ]?(\d{4})$/; // 添加电话号码正则表达式
  const mobile = tData.form.mobile; // 获取电话号码
  const phonePrefix = tData.form.phonePrefix; // 获取电话号码
  const county = tData.form.country;
  const postalCode = tData.form.postalCode;
  console.log(mobile)

  if (county == 'USA' || county == 'Canada') {
    //验证美国或加拿大手机号码，其他国家请自行验证
    if (!mobile || !phoneRegex.test(mobile)) { // 验证电话号码格式
      message.warn('Invalid phone number format!');
      return;
    }
  }
  if (postalCodeRegex.test(postalCode)) {
    message.warn('Invalid  postalCode!');
    return;

  }
  const creditCard = tData.form.creditCard;
  const cardType = tData.form.cardType;
  console.log(cardType + creditCard)

  if (cardType == "American Express") {
    console.log(cardType + creditCard)
    if (creditCard.length != 15 || !creditCard.toString().startsWith("34") || !creditCard.toString().startsWith("37")) {

      message.warn("Invalid creditCard")
      return;
    }
  }
  console.log("Starting code execution");
  if (cardType == "Visa") {
    console.log(cardType + creditCard)
    console.log( creditCard.length)
    console.log( creditCard.toString().startsWith('4'))


    if (creditCard.length != 16 || !creditCard.toString().startsWith('4')) {
      message.warn("Invalid creditCard")
      return;
    }
  }
  console.log("Ending code execution");

  if (cardType == "MasterCard") {
    if (creditCard.length != 16 || !creditCard.toString().startsWith("51")
      || !creditCard.startsWith("52")
      || !creditCard.startsWith("53")
      || !creditCard.startsWith("54")
      || !creditCard.startsWith("55")) {
      message.warn("Invalid creditCard")
      return;

    }
  }

  let formData = new FormData()
  let userId = userStore.user_id
  formData.append('id', userId)
  if (tData.form.avatarFile) {
    formData.append('avatarFile', tData.form.avatarFile)
  }
  if (tData.form.nickname) {
    formData.append('nickname', tData.form.nickname)
  }
  if (tData.form.email) {
    formData.append('email', tData.form.email)
  }
  if (tData.form.mobile) {
    formData.append('mobile', tData.form.mobile)
  }
  if (tData.form.phonePrefix) {
    formData.append('phonePrefix', tData.form.phonePrefix)
  }
  if (tData.form.firstName) {
    formData.append('firstName', tData.form.firstName)
  }
  if (tData.form.lastName) {
    formData.append('lastName', tData.form.lastName)
  }
  if (tData.form.country) {
    formData.append('country', tData.form.country)
  }
  if (tData.form.city) {
    formData.append('city', tData.form.city)
  }
  if (tData.form.state) {
    formData.append('state', tData.form.state)
  }
  if (tData.form.postalCode) {
    formData.append('postalCode', tData.form.postalCode)
  }
  if (tData.form.cardType) {
    formData.append('cardType', tData.form.cardType)
  }
  if (tData.form.creditCard) {
    formData.append('creditCard', tData.form.creditCard)
  }
  if (tData.form.expirationDate) {
    formData.append('expirationDate', tData.form.expirationDate)
  }
  if (tData.form.holder) {
    formData.append('holder', tData.form.holder)
  }
  if (tData.form.description) {
    formData.append('description', tData.form.description)
  }
  updateUserInfoApi(formData).then(res => {
    message.success('success')
    getUserInfo()
  }).catch(err => {
    console.log(err)
  })
}

</script>

<style scoped lang="less">
input,
textarea {
  border-style: none;
  outline: none;
  margin: 0;
  padding: 0;
}

.flex-view {
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
}

.content-list {
  -webkit-box-flex: 1;
  -ms-flex: 1;
  flex: 1;

  .list-title {
    color: #152844;
    font-weight: 600;
    font-size: 18px;
    line-height: 48px;
    height: 48px;
    margin-bottom: 4px;
    border-bottom: 1px solid #cedce4;
  }

  .edit-view {
    .item {
      -webkit-box-align: center;
      -ms-flex-align: center;
      align-items: center;
      margin: 24px 0;

      .label {
        width: 100px;
        color: #152844;
        font-weight: 600;
        font-size: 14px;
      }

      .right-box {
        -webkit-box-flex: 1;
        -ms-flex: 1;
        flex: 1;
      }

      .avatar {
        width: 64px;
        height: 64px;
        border-radius: 50%;
        margin-right: 16px;
      }

      .change-tips {
        -webkit-box-align: center;
        -ms-flex-align: center;
        align-items: center;
        -ms-flex-wrap: wrap;
        flex-wrap: wrap;
      }

      label {
        color: #4684e2;
        font-size: 14px;
        line-height: 22px;
        height: 22px;
        cursor: pointer;
        width: 100px;
        display: block;
      }

      .tip {
        color: #6f6f6f;
        font-size: 14px;
        height: 22px;
        line-height: 22px;
        margin: 0;
        width: 100%;
      }

      .right-box {
        -webkit-box-flex: 1;
        -ms-flex: 1;
        flex: 1;
      }

      .input-dom {
        width: 400px;
      }

      .input-dom {
        background: #f8fafb;
        border-radius: 4px;
        height: 40px;
        line-height: 40px;
        font-size: 14px;
        color: #152844;
        padding: 0 12px;
      }

      .tip {
        font-size: 12px;
        line-height: 16px;
        color: #6f6f6f;
        height: 16px;
        margin-top: 4px;
      }

      .intro {
        resize: none;
        background: #f8fafb;
        width: 100%;
        padding: 8px 12px;
        height: 82px;
        line-height: 22px;
        font-size: 14px;
        color: #152844;
      }
    }

    .save {
      background: #4684e2;
      border-radius: 32px;
      width: 96px;
      height: 32px;
      line-height: 32px;
      font-size: 14px;
      color: #fff;
      border: none;
      outline: none;
      cursor: pointer;
    }

    .mg {
      margin-left: 100px;
    }
  }
}
</style>
