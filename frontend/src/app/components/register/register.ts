import { ChangeDetectionStrategy, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-register',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './register.html',
    styleUrl: './register.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
    })
    export class RegisterComponent {
    submitted = false;
    fullName = '';
    email = '';
    legajo = '';

    submit(): void {
        this.submitted = true;
    }
}