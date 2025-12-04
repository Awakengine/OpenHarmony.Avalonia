# DBManager - 多数据库管理工具

一个基于 Avalonia UI 的跨平台数据库管理工具，支持 SQL Server、MySQL、PostgreSQL 和 Oracle 数据库，具有类似 Navicat 的用户界面。

## 功能特性

- 🔄 **多数据库支持**: 完全兼容 SQL Server、MySQL、PostgreSQL 和 Oracle
- 🖥️ **跨平台**: 支持 Windows、Linux 和 macOS
- 📁 **对象浏览器**: 直观的数据库对象树形结构浏览
- 🔍 **SQL 查询编辑器**: 强大的 SQL 编辑和执行功能
- 📊 **数据表编辑**: 直接编辑表中数据
- 📤 **导入导出**: 支持 CSV、JSON 等格式的数据导入导出
- 🎨 **现代化界面**: 类似 Navicat 的直观用户界面

## 项目结构

```
DBManager/
├── DBManager.csproj              # 核心项目文件
├── App.axaml                    # 应用程序入口点
├── ViewLocator.cs               # 视图定位器
├── Models/                      # 数据模型
│   ├── ConnectionInfo.cs        # 数据库连接信息模型
│   └── DatabaseType.cs          # 数据库类型枚举
├── Services/                    # 业务逻辑层
│   ├── DatabaseAdapterFactory.cs # 数据库适配器工厂
│   ├── DatabaseService.cs        # 数据库服务
│   ├── ImportExportService.cs   # 导入导出服务
│   └── DatabaseCompatibilityTestService.cs # 兼容性测试服务
├── ViewModels/                  # MVVM 视图模型
│   ├── ViewModelBase.cs         # 视图模型基类
│   ├── MainViewModel.cs         # 主窗口视图模型
│   ├── DatabaseObjectViewModel.cs # 数据库对象浏览器视图模型
│   ├── QueryEditorViewModel.cs  # 查询编辑器视图模型
│   └── TableEditorViewModel.cs  # 表编辑器视图模型
└── Views/                       # 用户界面
    ├── MainWindow.axaml         # 主窗口
    ├── DatabaseObjectBrowser.axaml # 数据库对象浏览器
    ├── QueryEditor.axaml        # 查询编辑器
    ├── TableEditor.axaml        # 表编辑器
    └── ImportExportDialog.axaml # 导入导出对话框
```

## 技术栈

- **Avalonia UI**: 跨平台 UI 框架
- **ReactiveUI**: MVVM 框架
- **.NET 8**: 运行时环境
- **数据库驱动**:
  - System.Data.SqlClient (SQL Server)
  - MySql.Data (MySQL)
  - Npgsql (PostgreSQL)
  - Oracle.ManagedDataAccess.Core (Oracle)

## 快速开始

1. 克隆项目:
   ```
   git clone <repository-url>
   ```

2. 还原 NuGet 包:
   ```
   dotnet restore
   ```

3. 构建项目:
   ```
   dotnet build
   ```

4. 运行桌面应用:
   ```
   cd DBManager.Desktop
   dotnet run
   ```

## 使用说明

1. **添加数据库连接**:
   - 点击"文件" -> "新建连接"
   - 选择数据库类型并填写连接信息
   - 测试连接后保存

2. **浏览数据库对象**:
   - 在左侧连接列表中选择数据库连接
   - 展开树形结构查看数据库、表、视图等对象

3. **执行 SQL 查询**:
   - 切换到"查询"标签页
   - 在文本框中编写 SQL 语句
   - 按 F5 或点击"执行"按钮运行查询

4. **编辑表数据**:
   - 在对象浏览器中双击表名
   - 在数据网格中直接编辑数据
   - 点击"保存更改"提交修改

5. **导入导出数据**:
   - 右键点击表名选择"导入/导出"
   - 选择相应选项完成数据传输

## 兼容性测试

项目包含完整的数据库兼容性测试套件，可验证各种数据库的连接和基本功能。

## 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情。

## 贡献

欢迎提交 Issue 和 Pull Request 来改进这个项目。