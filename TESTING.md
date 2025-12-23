# Hotel Management System - Testing Guide

## 测试指南 / Testing Guide

本文档提供了测试酒店管理系统的详细步骤。
This document provides detailed steps to test the Hotel Management System.

## 前置条件 / Prerequisites

1. MySQL服务器已安装并运行
2. 创建数据库：
```sql
CREATE DATABASE hotel_management;
```

3. 赋予用户权限：
```sql
GRANT ALL PRIVILEGES ON hotel_management.* TO 'root'@'localhost';
FLUSH PRIVILEGES;
```

## 测试场景 / Test Scenarios

### 测试1: 数据库连接测试 / Test 1: Database Connection

**目的**: 验证系统能够成功连接到MySQL数据库

**步骤**:
1. 运行程序: `dotnet run`
2. 选择使用默认连接设置或输入自定义设置
3. 观察连接状态消息

**预期结果**:
- 显示 "Connected successfully!"
- 显示 "Database tables created successfully."
- 显示 "Sample data inserted successfully."

---

### 测试2: 房间管理测试 / Test 2: Room Management

#### 2.1 查看所有房间
**步骤**:
1. 主菜单选择: 1 (Room Management)
2. 选择: 1 (View all rooms)

**预期结果**:
- 显示8个示例房间
- 房间101、102、201、202、301、302、401状态为Available
- 房间103状态为Maintenance

#### 2.2 添加新房间
**步骤**:
1. 主菜单选择: 1 (Room Management)
2. 选择: 3 (Add new room)
3. 输入信息:
   - Room Number: 104
   - Room Type: Standard
   - Price per Night: 100
   - Floor: 1
   - Max Occupancy: 2
   - Description: Test room

**预期结果**:
- 显示 "Room added successfully!"
- 再次查看房间列表应该能看到新房间

#### 2.3 更新房间
**步骤**:
1. 主菜单选择: 1 (Room Management)
2. 选择: 4 (Update room)
3. 输入房间号: 104
4. 更新价格为: 110
5. 状态保持不变

**预期结果**:
- 显示 "Room updated successfully!"
- 查看房间列表确认价格已更新

---

### 测试3: 客户管理测试 / Test 3: Guest Management

#### 3.1 注册新客户
**步骤**:
1. 主菜单选择: 2 (Guest Management)
2. 选择: 2 (Register new guest)
3. 输入信息:
   - First Name: Zhang
   - Last Name: San
   - ID Number: 123456789012345678
   - Phone: 13800138000
   - Email: zhangsan@example.com
   - Address: Beijing, China

**预期结果**:
- 显示 "Guest registered successfully!"
- 显示新的Guest ID

#### 3.2 查看客户列表
**步骤**:
1. 主菜单选择: 2 (Guest Management)
2. 选择: 1 (View all guests)

**预期结果**:
- 显示刚注册的客户信息

#### 3.3 搜索客户
**步骤**:
1. 主菜单选择: 2 (Guest Management)
2. 选择: 4 (Search guest)
3. 输入ID Number: 123456789012345678

**预期结果**:
- 显示客户完整信息

---

### 测试4: 预订管理测试 / Test 4: Reservation Management

#### 4.1 创建预订
**步骤**:
1. 主菜单选择: 3 (Reservation Management)
2. 选择: 2 (Make new reservation)
3. 输入信息:
   - Guest ID: 1 (使用之前注册的客户ID)
   - Room Number: 101
   - Check-in Date: 2025-12-25
   - Check-out Date: 2025-12-28
   - Special Requests: Late check-in

**预期结果**:
- 显示 "Reservation created successfully!"
- 显示Reservation ID
- 显示总金额 (应该是 3天 × $100 = $300)
- 房间101状态变为Occupied

#### 4.2 办理入住
**步骤**:
1. 主菜单选择: 3 (Reservation Management)
2. 选择: 3 (Check-in)
3. 输入Reservation ID: 1

**预期结果**:
- 显示 "Check-in successful!"
- 预订状态更新为CheckedIn

#### 4.3 办理退房
**步骤**:
1. 主菜单选择: 3 (Reservation Management)
2. 选择: 4 (Check-out)
3. 输入Reservation ID: 1

**预期结果**:
- 显示 "Check-out successful!"
- 预订状态更新为CheckedOut
- 房间101状态变回Available

#### 4.4 创建并取消预订
**步骤**:
1. 创建新预订 (Guest ID: 1, Room: 102)
2. 主菜单选择: 3 (Reservation Management)
3. 选择: 5 (Cancel reservation)
4. 输入新的Reservation ID
5. 确认: yes

**预期结果**:
- 显示 "Reservation cancelled successfully!"
- 预订状态更新为Cancelled
- 房间102状态变回Available

---

### 测试5: 报表功能测试 / Test 5: Reports

#### 5.1 房间状态报表
**步骤**:
1. 主菜单选择: 4 (Reports)
2. 选择: 1 (Room Status Report)

**预期结果**:
- 显示所有房间的详细状态信息
- 格式化的表格输出

#### 5.2 客户列表报表
**步骤**:
1. 主菜单选择: 4 (Reports)
2. 选择: 2 (Guest List)

**预期结果**:
- 显示所有已注册客户
- 包含客户ID、姓名、证件号、电话

#### 5.3 预订报表
**步骤**:
1. 主菜单选择: 4 (Reports)
2. 选择: 3 (Reservations Report)

**预期结果**:
- 显示所有预订记录
- 包含预订ID、客户ID、房间ID、日期、金额、状态

---

## 数据库直接验证 / Direct Database Verification

可以使用MySQL客户端直接查询数据库来验证数据：

```sql
USE hotel_management;

-- 查看所有表
SHOW TABLES;

-- 查看房间
SELECT * FROM Rooms;

-- 查看客户
SELECT * FROM Guests;

-- 查看预订
SELECT * FROM Reservations;

-- 查看完整的预订信息（带关联）
SELECT 
    r.ReservationId,
    g.FirstName,
    g.LastName,
    rm.RoomNumber,
    rm.RoomType,
    r.CheckInDate,
    r.CheckOutDate,
    r.TotalAmount,
    r.Status
FROM Reservations r
JOIN Guests g ON r.GuestId = g.GuestId
JOIN Rooms rm ON r.RoomId = rm.RoomId;
```

---

## 错误场景测试 / Error Scenario Testing

### 测试6: 错误处理

#### 6.1 预订不可用房间
**步骤**:
1. 尝试预订状态为Occupied或Maintenance的房间

**预期结果**:
- 显示错误消息："Room is not available for reservation."

#### 6.2 删除不存在的记录
**步骤**:
1. 尝试删除不存在的房间号

**预期结果**:
- 显示 "Room not found."

#### 6.3 无效输入
**步骤**:
1. 在需要数字的地方输入字母

**预期结果**:
- 显示错误消息并不崩溃

---

## 性能测试建议 / Performance Testing Suggestions

1. **批量数据测试**: 插入大量房间和客户数据，测试查询性能
2. **并发测试**: 同时进行多个预订操作
3. **索引效果**: 验证数据库索引对查询速度的影响

---

## 测试检查清单 / Testing Checklist

- [ ] 数据库连接成功
- [ ] 自动创建表结构
- [ ] 插入示例数据
- [ ] 查看所有房间
- [ ] 添加新房间
- [ ] 更新房间信息
- [ ] 删除房间
- [ ] 注册新客户
- [ ] 查看客户列表
- [ ] 搜索客户
- [ ] 更新客户信息
- [ ] 创建预订
- [ ] 办理入住
- [ ] 办理退房
- [ ] 取消预订
- [ ] 查看各类报表
- [ ] 错误处理测试
- [ ] 数据完整性验证

---

## 已知限制 / Known Limitations

1. 不支持同一房间的重叠预订检查
2. 没有实现支付处理
3. 没有用户认证和权限管理
4. 控制台界面在某些终端可能显示不正确

---

## 测试总结报告模板 / Test Summary Report Template

```
测试日期: ___________
测试人员: ___________
测试环境:
  - OS: ___________
  - .NET Version: ___________
  - MySQL Version: ___________

测试结果:
  通过: ___ / ___
  失败: ___ / ___

问题记录:
1. ___________
2. ___________

建议:
1. ___________
2. ___________
```
