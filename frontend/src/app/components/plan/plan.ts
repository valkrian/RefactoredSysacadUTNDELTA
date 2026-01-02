import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { PlanService } from '../../services/plan.service';
import { Plan } from '../../models/plan.model';

@Component({
  selector: 'app-plan',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './plan.html',
  styleUrl: './plan.css',
})
export class PlanComponent implements OnInit {
  readonly plan = signal<Plan | null>(null);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  constructor(private readonly planService: PlanService) {}

  ngOnInit(): void {
    this.planService.getMyPlan().subscribe({
      next: (data) => {
        this.plan.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('No se pudo cargar el plan de estudios.');
        this.loading.set(false);
      },
    });
  }
}
