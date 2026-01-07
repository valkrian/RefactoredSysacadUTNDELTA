export interface ExamCall {
  examCallId: number;
  subjectId: number;
  subjectName: string;
  startsAt: string;
  endsAt: string;
  capacity: number;
  enrolledCount: number;
}

export interface ExamEnrollment {
  examCallId: number;
  subjectId: number;
  subjectName: string;
  startsAt: string;
  endsAt: string;
  status: string;
  enrolledAt: string;
}
