import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { TaskItem, TaskPayload } from './models';

@Injectable({ providedIn: 'root' })
export class TaskService {
  private readonly http = inject(HttpClient);
  private readonly endpoint = '/api/tasks';

  list() { return this.http.get<TaskItem[]>(this.endpoint); }
  create(payload: TaskPayload) { return this.http.post<TaskItem>(this.endpoint, payload); }
  update(id: string, payload: Required<TaskPayload>) {
    return this.http.put<TaskItem>(`${this.endpoint}/${id}`, payload);
  }
  delete(id: string) { return this.http.delete<void>(`${this.endpoint}/${id}`); }
}
