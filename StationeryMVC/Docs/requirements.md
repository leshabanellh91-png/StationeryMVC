# StationeryMVC – System Requirements and Architecture

## 1. System Overview
StationeryMVC is a web-based application developed using **ASP.NET Core MVC**.
The system is designed to manage stationery items, generate QR codes,
create customer quotations, and manage shop settings through a centralized dashboard.

---

## 2. Functional Requirements

1. The system shall allow users to add, edit, view, and delete stationery items.
2. The system shall store stationery item details including name, category, quantity, price, image, and QR code.
3. The system shall generate a QR code for each stationery item.
4. The system shall allow users to create quotations for customers.
5. The system shall calculate the total amount of a quotation automatically.
6. The system shall store quotation date and customer details.
7. The system shall allow administrators to manage shop settings such as shop name, slogan, and logo.
8. The system shall display a dashboard with summary information such as total items and quotations.

---

## 3. Non-Functional Requirements

1. The system shall be easy to use and user-friendly.
2. The system shall validate all user input.
3. The system shall store data securely using a relational database.
4. The system shall provide acceptable response time for all operations.
5. The system shall be maintainable, scalable, and extensible.
6. The system shall follow MVC architectural best practices.

---

## 4. Entity Relationship Diagram (ERD)

```mermaid
erDiagram
    STATIONERY_ITEM ||--o{ QUOTATION_ITEM : included_in
    QUOTATION ||--|{ QUOTATION_ITEM : contains
    CUSTOMER ||--o{ QUOTATION : receives

    STATIONERY_ITEM {
        int ItemId
        string Name
        string Category
        int Quantity
        decimal Price
        string ImagePath
        string QrCode
    }

    CUSTOMER {
        int CustomerId
        string CustomerName
        string ContactNumber
        string Email
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

    SHOP_SETTING {
        int ShopSettingId
        string ShopName
        string Slogan
        string LogoPath
    }
