-- Hotel Management System Database Schema
-- MySQL Database

-- Create database
CREATE DATABASE IF NOT EXISTS hotel_management;
USE hotel_management;

-- Create Rooms table
CREATE TABLE IF NOT EXISTS Rooms (
    RoomId INT AUTO_INCREMENT PRIMARY KEY,
    RoomNumber VARCHAR(10) UNIQUE NOT NULL,
    RoomType VARCHAR(50) NOT NULL,
    PricePerNight DECIMAL(10,2) NOT NULL,
    Status VARCHAR(20) DEFAULT 'Available',
    Floor INT NOT NULL,
    MaxOccupancy INT NOT NULL,
    Description TEXT,
    INDEX idx_room_number (RoomNumber),
    INDEX idx_status (Status)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create Guests table
CREATE TABLE IF NOT EXISTS Guests (
    GuestId INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    IdNumber VARCHAR(50) UNIQUE NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    Email VARCHAR(100),
    Address VARCHAR(255),
    RegistrationDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_id_number (IdNumber),
    INDEX idx_phone (Phone)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create Reservations table
CREATE TABLE IF NOT EXISTS Reservations (
    ReservationId INT AUTO_INCREMENT PRIMARY KEY,
    GuestId INT NOT NULL,
    RoomId INT NOT NULL,
    CheckInDate DATE NOT NULL,
    CheckOutDate DATE NOT NULL,
    Status VARCHAR(20) DEFAULT 'Pending',
    TotalAmount DECIMAL(10,2) NOT NULL,
    ReservationDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    SpecialRequests TEXT,
    FOREIGN KEY (GuestId) REFERENCES Guests(GuestId) ON DELETE CASCADE,
    FOREIGN KEY (RoomId) REFERENCES Rooms(RoomId) ON DELETE CASCADE,
    INDEX idx_guest (GuestId),
    INDEX idx_room (RoomId),
    INDEX idx_status (Status),
    INDEX idx_dates (CheckInDate, CheckOutDate)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Create Payments table
CREATE TABLE IF NOT EXISTS Payments (
    PaymentId INT AUTO_INCREMENT PRIMARY KEY,
    ReservationId INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    PaymentDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    PaymentMethod VARCHAR(20) NOT NULL,
    Status VARCHAR(20) DEFAULT 'Pending',
    TransactionId VARCHAR(100),
    FOREIGN KEY (ReservationId) REFERENCES Reservations(ReservationId) ON DELETE CASCADE,
    INDEX idx_reservation (ReservationId),
    INDEX idx_status (Status)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Insert sample data
INSERT INTO Rooms (RoomNumber, RoomType, PricePerNight, Status, Floor, MaxOccupancy, Description) VALUES
('101', 'Standard', 100.00, 'Available', 1, 2, 'Comfortable standard room with city view'),
('102', 'Standard', 100.00, 'Available', 1, 2, 'Comfortable standard room with city view'),
('201', 'Deluxe', 150.00, 'Available', 2, 2, 'Spacious deluxe room with premium amenities'),
('202', 'Deluxe', 150.00, 'Available', 2, 2, 'Spacious deluxe room with premium amenities'),
('301', 'Suite', 250.00, 'Available', 3, 4, 'Luxurious suite with separate living area'),
('302', 'Suite', 250.00, 'Available', 3, 4, 'Luxurious suite with separate living area'),
('401', 'Presidential', 500.00, 'Available', 4, 6, 'Top-floor presidential suite with panoramic views'),
('103', 'Standard', 100.00, 'Maintenance', 1, 2, 'Currently under maintenance');
