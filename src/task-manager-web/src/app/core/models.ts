export type TaskStatus = 'Pending' | 'InProgress' | 'Completed';

export interface TaskItem {
  id: string;
  ownerId: string;
  title: string;
  description: string;
  status: TaskStatus;
  dueDate: string;
}

export interface AuthResult {
  userId: string;
  name: string;
  email: string;
  accessToken: string;
  expiresAt: string;
}

export interface TaskPayload {
  title: string;
  description: string;
  dueDate: string;
  status?: TaskStatus;
}
