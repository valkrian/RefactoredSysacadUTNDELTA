# 0004 - Domain rules and status enums

Date: 2025-12-31

## Context
Day 2 requires core domain rules for correlativities and explicit status handling
for enrollments and exam results. We also need a testable, framework-free model
in the Domain layer.

## Decision
- Create separate enums per context:
  - CourseEnrollmentStatus (Pending, Enrolled, Regular)
  - ExamEnrollmentStatus (Pending, Enrolled)
  - ExamResultStatus (Approved, Failed)
  - MinimumSubjectStatus (Regular, Approved) for prerequisite checks
- Add core entities needed by rules: Prerequisite, CourseEnrollment,
  ExamCall, ExamEnrollment, ExamResult.
- Implement a domain service (CorrelativityRules) that:
  - validates course enrollment using prerequisites marked ForCourse
  - validates exam enrollment using prerequisites marked ForExam
  - treats Approved as stronger than Regular
- Add unit tests for the correlativity rules in a dedicated test project.

## Consequences
- Domain remains independent of EF and API concerns.
- Status types are explicit per use case, reducing invalid states.
- Tests define expected behavior for future infrastructure and application layers.
