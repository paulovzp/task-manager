import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { AuthService } from '../../core/auth.service';
import { TaskItem, TaskStatus } from '../../core/models';
import { TaskService } from '../../core/task.service';

@Component({
  selector: 'app-tasks',
  imports: [DatePipe, ReactiveFormsModule],
  templateUrl: './tasks.component.html',
  styleUrl: './tasks.component.scss',
})
export class TasksComponent implements OnInit {
  private readonly taskService = inject(TaskService);
  private readonly formBuilder = inject(FormBuilder);
  private readonly router = inject(Router);
  readonly auth = inject(AuthService);
  readonly tasks = signal<TaskItem[]>([]);
  readonly loading = signal(true);
  readonly saving = signal(false);
  readonly error = signal('');
  readonly selectedTask = signal<TaskItem | null>(null);
  readonly filter = signal<TaskStatus | 'All'>('All');
  readonly formVisible = signal(false);
  readonly filteredTasks = computed(() => {
    const filter = this.filter();
    return this.tasks()
      .filter((task) => filter === 'All' || task.status === filter)
      .sort((left, right) => left.dueDate.localeCompare(right.dueDate));
  });
  readonly completedCount = computed(() => this.tasks().filter((task) => task.status === 'Completed').length);
  readonly dueSoonCount = computed(() => {
    const cutoff = Date.now() + 3 * 24 * 60 * 60 * 1000;
    return this.tasks().filter((task) => task.status !== 'Completed' && new Date(task.dueDate).getTime() <= cutoff).length;
  });
  readonly form = this.formBuilder.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    description: ['', [Validators.required, Validators.maxLength(2000)]],
    dueDate: ['', Validators.required],
    status: ['Pending' as TaskStatus, Validators.required],
  });

  ngOnInit(): void { this.loadTasks(); }

  loadTasks(): void {
    this.loading.set(true);
    this.error.set('');
    this.taskService.list().pipe(finalize(() => this.loading.set(false))).subscribe({
      next: (tasks) => this.tasks.set(tasks),
      error: (error: HttpErrorResponse) => this.handleError(error),
    });
  }

  openCreate(): void {
    this.selectedTask.set(null);
    const tomorrow = new Date(Date.now() + 24 * 60 * 60 * 1000);
    this.form.reset({ title: '', description: '', dueDate: this.toLocalInput(tomorrow), status: 'Pending' });
    this.formVisible.set(true);
  }

  openEdit(task: TaskItem): void {
    if (task.status === 'Completed') return;

    this.selectedTask.set(task);
    this.form.reset({
      title: task.title,
      description: task.description,
      dueDate: this.toLocalInput(new Date(task.dueDate)),
      status: task.status,
    });
    this.formVisible.set(true);
  }

  closeForm(): void { this.formVisible.set(false); this.selectedTask.set(null); }

  submit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    this.saving.set(true);
    this.error.set('');
    const value = this.form.getRawValue();
    const payload = { ...value, dueDate: new Date(value.dueDate).toISOString() };
    const request = this.selectedTask()
      ? this.taskService.update(this.selectedTask()!.id, payload)
      : this.taskService.create({ title: payload.title, description: payload.description, dueDate: payload.dueDate });
    request.pipe(finalize(() => this.saving.set(false))).subscribe({
      next: () => { this.closeForm(); this.loadTasks(); },
      error: (error: HttpErrorResponse) => this.handleError(error),
    });
  }

  deleteTask(task: TaskItem): void {
    if (!window.confirm(`Delete “${task.title}”? This cannot be undone.`)) return;
    this.taskService.delete(task.id).subscribe({
      next: () => this.tasks.update((tasks) => tasks.filter((item) => item.id !== task.id)),
      error: (error: HttpErrorResponse) => this.handleError(error),
    });
  }

  setFilter(filter: TaskStatus | 'All'): void { this.filter.set(filter); }

  logout(): void { this.auth.logout(); void this.router.navigate(['/login']); }

  private handleError(error: HttpErrorResponse): void {
    if (error.status === 401) { this.logout(); return; }
    this.error.set(error.error?.detail ?? 'Something went wrong. Please try again.');
  }

  private toLocalInput(date: Date): string {
    const local = new Date(date.getTime() - date.getTimezoneOffset() * 60_000);
    return local.toISOString().slice(0, 16);
  }
}
