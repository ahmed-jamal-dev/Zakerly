# Product Requirements Document

| **Field** | **Value** |
| --- | --- |
| Project Name | Zakerly |
| Version | 1.0 |
| Status | Draft |
| Authors | Benho Jr |
| Date | July 2026 |

# **Project Overview**

**Zakerly is a web-based Learning Management System (LMS) that enables instructors to create, organize, and manage courses while allowing students to enroll in courses, access learning materials, submit assignments, and track their academic progress. Administrators oversee users, courses, and platform operations to ensure the system runs efficiently and securely.**

# **Problem Statement**

Many educational institutions still rely on scattered communication channels and manual processes to distribute learning materials, collect assignments, and monitor student performance. These fragmented workflows make course management inefficient for both instructors and students. Zakerly addresses these challenges by providing a centralized platform for managing courses, learning resources, assignments, and academic progress in one place.

# **Project Goals**

1. Provide a centralized platform for online learning and course management.
2. Streamline course enrollment and the learning experience for students.
3. Enable instructors to create, organize, and manage course content.
4. Support assignment creation, submission, and grading.
5. Improve communication between instructors and students.
6. Enable administrators to efficiently manage users, courses, and platform operations.

# **Stakeholders**

| **Stakeholder** | **Description** |
| --- | --- |
| Students | Use the platform to enroll in courses, access learning materials, submit assignments, and track their academic progress. |
| Instructors | Create and manage courses, upload learning materials, create assignments, and evaluate students. |
| Administrators | Manage users, courses, and platform settings while ensuring smooth system operation. |
| Educational Institution | Benefits from a centralized system for managing teaching and learning activities. |

# **👥 User Roles**

### **🎓 Student**

**Description:
A student is the primary learner who enrolls in courses, accesses educational content, submits assignments, and monitors academic progress.**

| **Permission** | **Description** |
| --- | --- |
| Register | Create a new student account. |
| Login | Sign in to the platform. |
| View Profile | View personal profile information. |
| Update Profile | Edit profile information. |
| Browse Courses | View all available courses. |
| Search Courses | Search for courses by title or category. |
| Enroll in Course | Join a course. |
| View My Courses | View all enrolled courses. |
| Access Lessons | Watch videos and read learning materials. |
| Download Resources | Download PDFs and course files (if available). |
| Submit Assignment | Upload assignment submissions before the deadline. |
| View Grades | View grades and instructor feedback. |

### **👨‍🏫 Instructor**

Description:
An Instructor creates and manages courses, delivers educational content, creates assignments, and evaluates student submissions.

| **Permission** | **Description** |
| --- | --- |
| Register | Create an instructor account. |
| Login | Sign in to the platform. |
| Manage Profile | Update personal information. |
| Create Course | Create a new course. |
| Edit Course | Update course details. |
| Delete Course | Remove a course (if allowed). |
| Publish Course | Make the course available to students. |
| Create Lessons | Add lessons to a course. |
| Upload Learning Materials | Upload videos, PDFs, and other resources. |
| Create Assignments | Create assignments for students. |
| View Submissions | Review submitted assignments. |
| Grade Assignments | Assign grades and provide feedback. |
| View Enrolled Students | View students enrolled in their courses. |

### **👨‍💼 Administrator**

**Description:**

An Administrator manages the platform, oversees users and courses, and ensures the system operates efficiently.

| **Permission** | **Description** |
| --- | --- |
| Login | Access the administration panel. |
| Manage Users | Create, update, deactivate, or remove user accounts. |
| Manage Courses | View and manage all courses. |
| View Reports | View platform statistics and reports. |
| Configure System | Manage application settings and configurations. |

# **Functional Requirements**

### **Authentication**

**Description:**

**The system shall provide secure authentication and account management for all users.**

| **ID** | **Requirement** |
| --- | --- |
| FR-1 | The system shall allow students and instructors to register. |
| FR-2 | The system shall allow all users to log in using email and password. |
| FR-3 | The system shall validate user credentials before granting access. |
| FR-4 | The system shall allow authenticated users to log out. |
| FR-5 | Passwords shall be securely stored using the BCrypt hashing algorithm. |

### **User Management**

**Description:**

**The User Management module enables users to manage their personal information while allowing administrators to oversee user accounts and maintain platform integrity.**

| **ID** | **Requirement** |
| --- | --- |
| FR-6 | The system shall allow users to view their profile information. |
| FR-7 | The system shall allow users to update their profile information. |
| FR-8 | The system shall allow users to change their password. |
| FR-9 | The system shall allow administrators to view all user accounts. |
| FR-10 | The system shall allow administrators to delete user accounts when necessary. |

### **Course Management**

**Description:**

The Course Management module enables instructors to create, organize, update, and manage courses, while allowing students to browse and enroll in available courses. Administrators have oversight capabilities to manage courses across the platform.

| **ID** | **Requirement** |
| --- | --- |
| FR-11 | The system shall allow instructors to create new courses. |
| FR-12 | The system shall allow instructors to edit course information. |
| FR-13 | The system shall allow instructors to publish or unpublish courses. |
| FR-14 | The system shall allow instructors to archive or delete their courses. |
| FR-15 | The system shall allow students to browse all published courses. |
| FR-16 | The system shall display detailed information for each course, including description, instructor, category, and enrollment status. |
| FR-17 | The system shall allow administrators to manage all courses on the platform. |

### **Lesson Management**

**Description:**

The Lesson Management module enables instructors to create and organize course lessons by uploading learning materials such as videos, documents, and external resources. Students can access these lessons after enrolling in the course.

| **ID** | **Requirement** |
| --- | --- |
| FR-18 | The system shall allow instructors to create lessons within a course. |
| FR-19 | The system shall allow instructors to edit lesson information. |
| FR-20 | The system shall allow instructors to delete lessons. |
| FR-21 | The system shall allow instructors to upload learning materials such as videos, PDFs, and documents. |
| FR-22 | The system shall allow students to view lessons for enrolled courses. |
| FR-23 | The system shall restrict lesson access to enrolled students only. |

### **Enrollment Management**

**Description:**

The Enrollment Management module allows students to enroll in available courses while enabling instructors and administrators to monitor course enrollments.

| **ID** | **Requirement** |
| --- | --- |
| FR-24 | The system shall allow students to enroll in published courses. |
| FR-25 | The system shall prevent duplicate enrollments. |
| FR-26 | The system shall allow students to view all enrolled courses. |
| FR-27 | The system shall allow instructors to view enrolled students for their courses. |
| FR-28 | The system shall allow administrators to manage enrollments when necessary. |

### **Assignment Management**

**Description:**

The Assignment Management module enables instructors to create assignments and evaluate student submissions while allowing students to submit their work before the deadline.

| **ID** | **Requirement** |
| --- | --- |
| FR-29 | The system shall allow instructors to create assignments. |
| FR-30 | The system shall allow instructors to define assignment deadlines. |
| FR-31 | The system shall allow students to submit assignments before the deadline. |
| FR-32 | The system shall prevent submissions after the deadline. |
| FR-33 | The system shall allow instructors to review submissions. |
| FR-34 | The system shall allow instructors to grade assignments and provide feedback. |
| FR-35 | The system shall allow students to view grades and instructor feedback. |

# **Scope**

### **In Scope**

- User Authentication
- User Management
- Course Management
- Lesson Management
- Enrollment Management
- Assignment Management

### **Out of Scope**

- Category Management
- Announcements
- Online payments
- Live video classes
- AI assistant
- Certificates
- Mobile application
- Discussion forums

# **Non-Functional Requirements**

| **Category** | **Requirement** |
| --- | --- |
| Performance | API response time should be less than 2 seconds under normal load. |
| Security | Passwords shall be hashed using BCrypt and authentication shall use JWT. |
| Availability | The system shall be available 99% of the time during normal operation. |
| Scalability | The system should support future expansion without major architectural changes. |
| Usability | The user interface should be simple and easy to navigate. |
| Reliability | The system shall prevent data loss during normal operation. |

# **Future Enhancements**

- Live virtual classrooms
- Online payments
- Certificates
- AI-powered recommendations
- Mobile application
- Push notifications