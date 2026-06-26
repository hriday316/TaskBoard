import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environment';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WorkspaceService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;

  workspaces = signal<any[]>([]);
  selectedWorkspace = signal<any>(null);

  getWorkspaces() {
    return this.http.get<any[]>(this.baseUrl + 'workspace', { withCredentials: true }).pipe(
      tap((workspaces) => {this.setWorkspaces(workspaces), console.log('Workspaces loaded successfull:', workspaces);})
    );
  }

  createWorkspace(value: { name: string; description?: string }) {
    return this.http.post(this.baseUrl + 'workspace', value, { withCredentials: true });
  }
  getWorkspaceMembers() {
    const workspaceId = this.selectedWorkspace()?.id;
    return this.http.get<any[]>(this.baseUrl + 'workspace/' + workspaceId + '/members', { withCredentials: true });
  }

  selectWorkspace(workspace: any) {
    this.selectedWorkspace.set(workspace);

    if (workspace) {
      localStorage.setItem('workspaceId', workspace.id);
    } else {
      localStorage.removeItem('workspaceId');
    }
  }

  setWorkspaces(workspaces: any[]) {
    this.workspaces.set(workspaces);

    const lastWorkspaceId = localStorage.getItem('workspaceId');
    const workspace = workspaces.find(x => x.id === lastWorkspaceId) || workspaces[0];

    if (workspace) {
      this.selectWorkspace(workspace);
    } else {
      this.selectWorkspace(null);
    }
  }

  addWorkspace(workspace: any) {
    this.workspaces.update(workspaces => [...workspaces, workspace]);
    this.selectWorkspace(workspace);
  }
}
