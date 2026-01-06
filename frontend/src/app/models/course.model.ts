export interface CourseOffer {
  courseOfferId: number;
  subjectId: number;
  code: string;
  subjectName: string;
  period: string;
}

export interface CourseEnrollment {
  subjectId: number;
  subjectName: string;
  period: string;
  status: string;
}
