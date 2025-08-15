# DVLD – Driving & Vehicle License Management System

A multi-layered Windows Forms application for managing driving license services. DVLD simulates the operations of a real Driving and Vehicle Licensing Department, enabling efficient handling of license requests, test workflows, renewals, replacements, and user administration.

---

## 🧱 Architecture

This project follows a 3-tier architecture:

- **Presentation Layer:** WinForms-based desktop UI
- **Business Logic Layer (BLL):** Validation and business rules
- **Data Access Layer (DAL):** SQL Server-based data access

---

## 📌 Features

### 👤 User & Person Management
- Add/edit/delete/search users
- Assign roles and freeze accounts
- Maintain personal data (Name, National No., DOB, Contact Info, Photo)

### 🚗 License Services
- Apply for license issuance (based on class & age)
- Renew licenses
- Replace lost or damaged licenses
- Revoke or suspend licenses
- Issue international licenses

### 📝 Request Tracking
- Submit, track, and manage service requests
- Prevent duplicates per service & person
- View request status: New, Completed, Cancelled

### 🧪 Test Management
- Schedule and perform: vision test → theoretical → practical
- Retests available for failed applicants
- Store test results, fees, and status

### 🔁 License Suspension / Release
- Suspend license with reason and date
- Re-activate after paying fines

---

## 💽 Database

- **Type:** SQL Server
- **Provided:** `.bak` file (database backup)
- **Schema:** Contains tables for persons, users, licenses, requests, tests, suspensions, etc.

To restore:
1. Open SQL Server Management Studio (SSMS)
2. Restore the database from the `DVLD.bak` file
3. Update `App.config` with your connection string

---

## 🛠 Technologies Used

- C# (.NET Framework)
- Windows Forms
- SQL Server
- ADO.NET - For database access (SQL Server)
- Layered architecture
  
---

## 🚀 Getting Started

1. Clone the repo
2. Restore the `.bak` file using SQL Server
3. Open `DVLD.sln` in Visual Studio
4. Update `App.config` with your DB connection string
5. Build and run
6. Sign in using username: Ziad, Password: 1234 (any Password is 1234).
---

## 📌 Notes

- A person can hold multiple licenses of different classes, but not multiple of the same.
- System prevents login after repeated failures (optional).
- All test steps must be passed in order: Vision → Theoretical → Practical.

---

## 📄 License

This project is provided for educational purposes only.
