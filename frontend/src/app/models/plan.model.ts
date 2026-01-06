export interface PlanSubject {
  subjectId: number;
  code: string;
  name: string;
  year: number;
  term: number;
}

export interface Plan {
  planId: number;
  planName: string;
  career: string;
  yearVersion: number;
  subjects: PlanSubject[];
}
