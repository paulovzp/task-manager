import { signal } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { of } from 'rxjs';
import { AuthService } from '../../core/auth.service';
import { TaskItem } from '../../core/models';
import { TaskService } from '../../core/task.service';
import { TasksComponent } from './tasks.component';

describe('TasksComponent', () => {
  const taskService = {
    list: () => of([]),
  };
  const authService = {
    user: signal(null),
    logout: () => undefined,
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TasksComponent],
      providers: [
        provideRouter([]),
        { provide: TaskService, useValue: taskService },
        { provide: AuthService, useValue: authService },
      ],
    }).compileComponents();
  });

  it('hides status while creating and shows it while editing', () => {
    const fixture = TestBed.createComponent(TasksComponent);
    const component = fixture.componentInstance;
    fixture.detectChanges();

    component.openCreate();
    fixture.detectChanges();

    expect(fixture.nativeElement.querySelector('#status')).toBeNull();

    const existingTask: TaskItem = {
      id: 'task-1',
      ownerId: 'user-1',
      title: 'Review pull request',
      description: 'Check the task manager changes',
      dueDate: '2026-09-05T12:00:00.000Z',
      status: 'InProgress',
    };
    component.openEdit(existingTask);
    fixture.detectChanges();

    expect(fixture.nativeElement.querySelector('#status')).not.toBeNull();
  });

  it('does not allow a completed task to open in the editor', () => {
    const fixture = TestBed.createComponent(TasksComponent);
    const component = fixture.componentInstance;
    fixture.detectChanges();
    component.loading.set(false);
    component.tasks.set([{
      id: 'task-2',
      ownerId: 'user-1',
      title: 'Completed task',
      description: 'This work is finished',
      dueDate: '2026-09-05T12:00:00.000Z',
      status: 'Completed',
    }]);
    fixture.detectChanges();

    const editButton = fixture.nativeElement.querySelector('.task-main') as HTMLButtonElement;

    expect(editButton.disabled).toBe(true);
    editButton.click();
    expect(component.formVisible()).toBe(false);
  });
});
