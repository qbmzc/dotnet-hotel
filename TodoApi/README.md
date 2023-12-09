# 基于asp.net的酒店预订系统设计与实现

## 开发环境搭建

下载并安装Visual Studio 2022,或使用vscode

安装.Net SDK8.0，安装配置环境并验证

```bash
dotnet --version

# 输出版本信息
# 8.0.100
```

## 安装node.js并验证

```bash
npm --version
```

## clone代码并导入VS



跨域配置

Program.cs

```c#
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://127.0.0.1:3000")//修改成你的前端运行地址和端口，如果有域名，则修改成域名
                                              .AllowAnyHeader();
                      });
});
```

数据库配置


1. 使用localDb

安装vs时会自动安装相关的数据库,默认位置在C盘当前用户下，以mdf结尾

![image](./image/WX20231207-171912.png)

2. 连接已经存在的SQL Server,没有的话 建议docker 安装
 [使用 Docker 运行 SQL Server Linux 容器映像](https://learn.microsoft.com/zh-cn/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&pivots=cs1-bash)

3. 在[TodoApi\appsettings.json](TodoApi\appsettings.json)中配置连接字符串`MyContext`

转义字符使用`\`
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "MyContext": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=hotel.db;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"
  }
}
```

查看连接字符串,使用vs连接数据库并查看属性

![Alt text](./image/image.png)

## 自动建表

### 确保已安装 Entity Framework Core 工具：

```bash
dotnet tool install --global dotnet-ef
```

## 尝试使用 dotnet 命令执行数据库更新：

```bash
# 使用 dotnet 命令执行迁移
# 如果已经执行过，则跳过，直接执行下一个命令更新数据库
dotnet ef migrations add InitialCreate

dotnet ef database update
```

## 运行后端

```bash
# 进入后端目录
cd TodoApi

dotnet run 
```


#### 前端运行步骤

1. 安装node

2. 修改web/src/store下的constansts.ts文件中的BASE_URL，改成你自己后端的地址,如果在同一台服务器上运行，则使用127.0.0.1:9100即可

3. cmd命令进入web目录下，安装依赖，执行:
```
npm install 
```
4. 运行项目
```
npm run dev
```
5. 在浏览器输入: http://localhost:3000 即可预览


