# ERD - Autogestion Alumnos (MVP)

This ERD covers the minimal schema for the MVP described in scope.md.

```mermaid
erDiagram
  plans {
    int id PK
    string name
    string career
    int year_version
  }
  students {
    int id PK
    string legajo
    string full_name
    string email
    string password_hash
    int plan_id FK
    datetime created_at
  }
  subjects {
    int id PK
    string code
    string name
    int year
    int term
  }
  plan_subjects {
    int plan_id FK
    int subject_id FK
  }
  prerequisites {
    int subject_id FK
    int requires_subject_id FK
    string requirement_type
    string min_status
  }
  course_enrollments {
    int id PK
    int student_id FK
    int subject_id FK
    string period
    string status
  }
  exam_calls {
    int id PK
    int subject_id FK
    datetime starts_at
    datetime ends_at
    int capacity
  }
  exam_enrollments {
    int id PK
    int student_id FK
    int exam_call_id FK
    string status
    datetime enrolled_at
  }
  exam_results {
    int id PK
    int student_id FK
    int subject_id FK
    date date
    int grade
    string result_status
  }

  plans ||--o{ students : has
  plans ||--o{ plan_subjects : includes
  subjects ||--o{ plan_subjects : included_in

  subjects ||--o{ prerequisites : requires
  subjects ||--o{ prerequisites : required_by

  students ||--o{ course_enrollments : enrolls
  subjects ||--o{ course_enrollments : offered

  subjects ||--o{ exam_calls : has
  exam_calls ||--o{ exam_enrollments : enrolls
  students ||--o{ exam_enrollments : submits

  students ||--o{ exam_results : earns
  subjects ||--o{ exam_results : results
```

## Notes
- `requirement_type`: FOR_COURSE or FOR_EXAM
- `min_status`: REGULAR or APPROVED (optional)
- Suggested unique constraints:
  - students.legajo
  - students.email
  - plan_subjects (plan_id, subject_id)
  - prerequisites (subject_id, requires_subject_id, requirement_type)
  - course_enrollments (student_id, subject_id, period)
  - exam_enrollments (student_id, exam_call_id)
