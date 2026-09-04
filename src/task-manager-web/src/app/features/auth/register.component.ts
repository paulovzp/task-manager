import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthService } from '../../core/auth.service';

@Component({ selector: 'app-register', imports: [ReactiveFormsModule, RouterLink], templateUrl: './register.component.html', styleUrl: './auth.scss' })
export class RegisterComponent {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);
  private readonly formBuilder = inject(FormBuilder);
  readonly busy = signal(false);
  readonly error = signal('');
  readonly form = this.formBuilder.nonNullable.group({
    name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(120)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$/)]],
  });

  submit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.busy.set(true);
    this.error.set('');
    const value = this.form.getRawValue();
    this.auth.register(value.name, value.email, value.password).pipe(finalize(() => this.busy.set(false))).subscribe({
      next: () => void this.router.navigate(['/tasks']),
      error: (error: HttpErrorResponse) => this.error.set(error.error?.detail ?? 'We could not create your account. Please review your details.'),
    });
  }
}
