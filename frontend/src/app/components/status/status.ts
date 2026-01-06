import { ChangeDetectionStrategy, Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StatusService } from '../../services/status.service';
import { StudentStatus } from '../../models/status.model';

@Component({
  selector: 'app-status',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './status.html',
  styleUrl: './status.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class StatusComponent implements OnInit {
  readonly status = signal<StudentStatus | null>(null);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  constructor(private readonly statusService: StatusService) {}

  ngOnInit(): void {
    this.statusService.getMyStatus().subscribe({
      next: (data) => {
        this.status.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('No se pudo cargar el estado academico.');
        this.loading.set(false);
      },
    });
  }
}
