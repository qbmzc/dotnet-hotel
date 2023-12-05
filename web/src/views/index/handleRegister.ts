import { userRegisterApi } from '/@/api/user';
import { message } from "ant-design-vue";
import { tData, router } from './register.vue';

export const handleRegister = () => {
console.log('login');
if (tData.loginForm.username === ''
|| tData.loginForm.password === ''
|| tData.loginForm.repassword === '') {
message.warn('Not null!');
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
message.warn('Invalid  characters!');
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
message.warn('Invalid  postalCode!');
}
const creDitCard = tData.loginForm.creDitCard;
tData.loginForm.cardType;
if ()
userRegisterApi({
username: tData.loginForm.username,
password: tData.loginForm.password,
rePassword: tData.loginForm.repassword,
mobile: tData.loginForm.mobile,
countryCode: phonePrefix + "-" + mobile // 添加国家代码到请求参数中
}).then(res => {
message.success('success!');
router.push({ name: 'login' });
}).catch(err => {
message.error(err.msg || 'failed');
});
};
