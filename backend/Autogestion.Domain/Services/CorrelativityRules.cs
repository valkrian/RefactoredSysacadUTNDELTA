using Autogestion.Domain.Entities;
using Autogestion.Domain.Enums;

namespace Autogestion.Domain.Services;

/// <summary>
/// Pure domain rules for correlativity checks.
/// </summary>
public static class CorrelativityRules
{
    /// <summary>
    /// Validates whether a student can enroll in a course based on prerequisites.
    /// </summary>
    public static bool CanEnrollCourse(
        int subjectId,
        IEnumerable<Prerequisite> prerequisites,
        IEnumerable<CourseEnrollment> courseEnrollments,
        IEnumerable<ExamResult> examResults)
    {
        var required = prerequisites
            .Where(p => p.SubjectId == subjectId && p.Type == PrerequisiteType.ForCourse);

        return ArePrerequisitesMet(required, courseEnrollments, examResults);
    }

    /// <summary>
    /// Validates whether a student can enroll in an exam based on prerequisites.
    /// </summary>
    public static bool CanEnrollExam(
        int subjectId,
        IEnumerable<Prerequisite> prerequisites,
        IEnumerable<CourseEnrollment> courseEnrollments,
        IEnumerable<ExamResult> examResults)
    {
        var required = prerequisites
            .Where(p => p.SubjectId == subjectId && p.Type == PrerequisiteType.ForExam);

        return ArePrerequisitesMet(required, courseEnrollments, examResults);
    }

    /// <summary>
    /// Checks that every required prerequisite is satisfied by the student history.
    /// </summary>
    private static bool ArePrerequisitesMet(
        IEnumerable<Prerequisite> required,
        IEnumerable<CourseEnrollment> courseEnrollments,
        IEnumerable<ExamResult> examResults)
    {
        foreach (var prerequisite in required)
        {
            if (prerequisite.MinimumStatus == MinimumSubjectStatus.Approved)
            {
                if (!HasApproved(prerequisite.RequiresSubjectId, examResults))
                {
                    return false;
                }
            }
            else
            {
                if (!HasRegularOrApproved(prerequisite.RequiresSubjectId, courseEnrollments, examResults))
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Returns true if the student has an approved result for the subject.
    /// </summary>
    private static bool HasApproved(int subjectId, IEnumerable<ExamResult> examResults)
    {
        return examResults.Any(r =>
            r.SubjectId == subjectId &&
            r.Status == ExamResultStatus.Approved);
    }

    /// <summary>
    /// Returns true if the student has regular or approved status for the subject.
    /// </summary>
    private static bool HasRegularOrApproved(
        int subjectId,
        IEnumerable<CourseEnrollment> courseEnrollments,
        IEnumerable<ExamResult> examResults)
    {
        if (HasApproved(subjectId, examResults))
        {
            return true;
        }

        return courseEnrollments.Any(c =>
            c.SubjectId == subjectId &&
            c.Status == CourseEnrollmentStatus.Regular);
    }
}
