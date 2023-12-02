# 基于asp.net的酒店预订系统设计与实现

> 学习过程中，遇到问题可以咨询作者。

### 演示地址

前台地址：  http://hotel.gitapp.cn

后台地址： http://hotel.gitapp.cn/admin

后台管理帐号：

```bash
用户名：admin123
密码：admin123
```
### 功能介绍

平台采用B/S结构，后端采用主流的`asp.net`框架进行开发，前端采用主流的`Vue.js`进行开发。

整个平台包括前台和后台两个部分。

- 前台功能包括：首页、房间详情页、订单、用户中心模块。
- 后台功能包括：总览、订单管理、房间管理、分类管理、标签管理、评论管理、用户管理、运营管理、日志管理、系统信息模块。



#### 前端运行步骤

1. 安装node 16.14

2. 修改web/src/store下的constansts.ts文件中的BASE_URL，改成你自己后端的地址

3. cmd命令进入web目录下，安装依赖，执行:
```
npm install 
```
4. 运行项目
```
npm run dev
```
5. 在浏览器输入: http://localhost:3000 即可预览


### 界面预览

首页

![](https://github.com/geeeeeeeek/java_hotel/blob/master/server/upload/image/a.png)


后台页面

![](https://github.com/geeeeeeeek/java_hotel/blob/master/server/upload/image/b.png)


