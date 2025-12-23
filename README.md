# Hotel Management System (酒店管理系统)

## 项目概述 / Project Overview

这是一个基于 .NET 8.0 和 MySQL 数据库的酒店管理系统。该系统实现了酒店日常运营所需的核心功能，包括房间管理、客户管理、预订管理和报表功能。

This is a Hotel Management System built with .NET 8.0 and MySQL database. The system implements core functionalities required for hotel daily operations, including room management, guest management, reservation management, and reporting features.

## 技术栈 / Technology Stack

- **Framework**: .NET 8.0
- **Database**: MySQL
- **Language**: C#
- **Database Connector**: MySql.Data 9.1.0

## 功能特性 / Features

### 1. 房间管理 / Room Management
- 查看所有房间状态
- 查看可用房间
- 添加新房间
- 更新房间信息
- 删除房间
- 房间状态管理（可用、已占用、维护中）

### 2. 客户管理 / Guest Management
- 客户注册
- 查看所有客户
- 更新客户信息
- 按证件号搜索客户
- 客户历史记录

### 3. 预订管理 / Reservation Management
- 创建新预订
- 办理入住（Check-in）
- 办理退房（Check-out）
- 取消预订
- 查看所有预订
- 自动计算费用

### 4. 报表功能 / Reports
- 房间状态报表
- 客户列表
- 预订报表

## 数据库结构 / Database Schema

### Tables

1. **Rooms** - 房间信息表
   - RoomId (Primary Key)
   - RoomNumber (Unique)
   - RoomType
   - PricePerNight
   - Status
   - Floor
   - MaxOccupancy
   - Description

2. **Guests** - 客户信息表
   - GuestId (Primary Key)
   - FirstName
   - LastName
   - IdNumber (Unique)
   - Phone
   - Email
   - Address
   - RegistrationDate

3. **Reservations** - 预订信息表
   - ReservationId (Primary Key)
   - GuestId (Foreign Key)
   - RoomId (Foreign Key)
   - CheckInDate
   - CheckOutDate
   - Status
   - TotalAmount
   - ReservationDate
   - SpecialRequests

4. **Payments** - 支付信息表
   - PaymentId (Primary Key)
   - ReservationId (Foreign Key)
   - Amount
   - PaymentDate
   - PaymentMethod
   - Status
   - TransactionId

## 安装和配置 / Installation and Configuration

### 前置要求 / Prerequisites

1. .NET 8.0 SDK or later
2. MySQL Server 5.7 or later
3. MySQL client (optional, for manual database setup)

### 数据库设置 / Database Setup

#### 方法1：使用SQL脚本 / Method 1: Using SQL Script

```bash
# 登录MySQL / Login to MySQL
mysql -u root -p

# 运行SQL脚本 / Run SQL script
source database_setup.sql
```

#### 方法2：自动初始化 / Method 2: Automatic Initialization

程序首次运行时会自动创建数据库表和示例数据。只需确保：
- MySQL服务正在运行
- 创建数据库：`CREATE DATABASE hotel_management;`
- 具有适当的用户权限

The program will automatically create database tables and sample data on first run. Just ensure:
- MySQL service is running
- Create database: `CREATE DATABASE hotel_management;`
- Have appropriate user permissions

### 编译和运行 / Build and Run

```bash
# 进入项目目录 / Navigate to project directory
cd HotelManagement

# 还原依赖 / Restore dependencies
dotnet restore

# 编译项目 / Build project
dotnet build

# 运行程序 / Run application
dotnet run
```

## 使用说明 / Usage

### 启动程序 / Starting the Application

1. 运行程序后，系统会提示输入数据库连接信息
2. 可以使用默认设置（localhost, hotel_management, root, 无密码）
3. 或者输入自定义的数据库连接参数

1. When the program starts, it will prompt for database connection information
2. You can use default settings (localhost, hotel_management, root, no password)
3. Or enter custom database connection parameters

### 默认连接设置 / Default Connection Settings

```
Server: localhost
Database: hotel_management
Username: root
Password: (empty)
```

### 主菜单 / Main Menu

程序提供以下主要功能模块：
The program provides the following main modules:

1. **房间管理 / Room Management**
2. **客户管理 / Guest Management**
3. **预订管理 / Reservation Management**
4. **报表 / Reports**
5. **退出 / Exit**

## 示例数据 / Sample Data

系统自动创建以下示例房间：
The system automatically creates the following sample rooms:

- Room 101-102: Standard rooms ($100/night)
- Room 201-202: Deluxe rooms ($150/night)
- Room 301-302: Suites ($250/night)
- Room 401: Presidential suite ($500/night)

## 项目结构 / Project Structure

```
HotelManagement/
├── Models/              # 数据模型 / Data models
│   ├── Room.cs
│   ├── Guest.cs
│   ├── Reservation.cs
│   └── Payment.cs
├── Data/                # 数据访问层 / Data access layer
│   ├── DatabaseConnection.cs
│   ├── DatabaseInitializer.cs
│   ├── RoomRepository.cs
│   ├── GuestRepository.cs
│   └── ReservationRepository.cs
├── Services/            # 业务逻辑层 / Business logic layer
│   └── HotelService.cs
└── Program.cs           # 主程序入口 / Main program entry
```

## 开发说明 / Development Notes

### 架构模式 / Architecture Pattern

本系统采用三层架构：
This system uses a three-tier architecture:

1. **表示层 (Presentation Layer)**: Console-based UI in Program.cs
2. **业务逻辑层 (Business Logic Layer)**: Services/
3. **数据访问层 (Data Access Layer)**: Data/ and Models/

### 设计模式 / Design Patterns

- **Repository Pattern**: 用于数据访问抽象
- **Dependency Injection**: 通过构造函数注入
- **Single Responsibility**: 每个类专注于单一职责

## 安全注意事项 / Security Considerations

1. 数据库密码应该加密存储
2. 应该使用参数化查询防止SQL注入（已实现）
3. 生产环境中不应使用空密码
4. 敏感数据应该加密传输

1. Database passwords should be encrypted
2. Use parameterized queries to prevent SQL injection (implemented)
3. Empty passwords should not be used in production
4. Sensitive data should be encrypted during transmission

## 故障排除 / Troubleshooting

### 无法连接到数据库 / Cannot Connect to Database

1. 确认MySQL服务正在运行
2. 检查数据库名称、用户名和密码
3. 确认MySQL端口（默认3306）未被防火墙阻止

### 表已存在错误 / Table Already Exists Error

程序使用 `CREATE TABLE IF NOT EXISTS`，不应出现此错误。如果出现，请检查数据库权限。

## 未来改进 / Future Improvements

- [ ] Web界面支持
- [ ] 支付系统集成
- [ ] 邮件通知功能
- [ ] 多语言支持
- [ ] 数据备份和恢复
- [ ] 高级报表和统计
- [ ] 用户权限管理

## 许可证 / License

本项目用于教学和学习目的。
This project is for educational and learning purposes.

## 联系方式 / Contact

如有问题或建议，请联系项目维护者。
For questions or suggestions, please contact the project maintainer.

---

**注意 / Note**: 本系统根据《.NET体系结构与应用开发》实验指导任务书开发，使用MySQL作为数据库。

**Note**: This system is developed according to the ".NET Architecture and Application Development" experimental instruction manual, using MySQL as the database.
