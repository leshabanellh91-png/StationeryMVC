# StationeryMVC – System Architecture

## 1. Architecture Overview
StationeryMVC follows the Model-View-Controller (MVC) architecture pattern.
This architecture separates the application into three main layers:
Model, View, and Controller.

## 2. MVC Architecture

### 2.1 Model Layer
The Model layer represents the application’s data and business rules.
It includes the following models:
- StationeryItem
- Quotation
- QuotationItem
- AppSettings
- ErrorViewModel

### 2.2 View Layer
The View layer is responsible for displaying data to the user.
It uses Razor views with HTML, CSS, and Bootstrap.

### 2.3 Controller Layer
The Controller layer handles user requests and business logic.
It communicates between the Model and View layers.

## 3. Technology Stack
- Frontend: HTML, CSS, Bootstrap, Razor Views
- Backend: ASP.NET Core MVC (C#)
- Database: SQL Server
- ORM: Entity Framework Core
- QR Code Generation: QR Code library

## 4. Entity Relationship Diagram (ERD)

The ERD illustrates the structure of the database and the relationships
between core entities used in the StationeryMVC system.

```mermaid
erDiagram
    STATIONERY_ITEM ||--o{ QUOTATION_ITEM : included_in
    QUOTATION ||--|{ QUOTATION_ITEM : contains

    STATIONERY_ITEM {
        int ItemId
        string Name
        string Category
        int Quantity
        decimal Price
        string ImagePath
        string QrCode
    }

    QUOTATION {
        int QuotationId
        date QuotationDate
        decimal TotalAmount
    }

    QUOTATION_ITEM {
        int QuotationItemId
        int Quantity
        decimal UnitPrice
    }

    APP_SETTINGS {
        int AppSettingsId
        string ShopName
        string Slogan
        string LogoPath
    }
