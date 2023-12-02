## 确保已安装 Entity Framework Core 工具：

```bash
dotnet tool install --global dotnet-ef

```

## 尝试使用 dotnet 命令执行数据库更新：

```bash
# 使用 dotnet 命令执行迁移
dotnet ef migrations add InitialCreate

dotnet ef database update
```