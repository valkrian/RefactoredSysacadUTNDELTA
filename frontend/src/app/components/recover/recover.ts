import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-recover',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './recover.html',
    styleUrl: './recover.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
    })
    export class RecoverComponent {
    submitted = false;
    email = '';

    submit(): void {
        this.submitted = true;
    }
}
