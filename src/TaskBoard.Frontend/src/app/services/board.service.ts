import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environment';

@Injectable({ providedIn: 'root' })
export class BoardService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;
  board = signal<any>(null);

  getBoards(workspaceId: string) {
    return this.http.get(this.baseUrl + 'workspaces/' + workspaceId + '/boards', { withCredentials: true });
  }

  getBoard(workspaceId: string, boardId: string) {
    return this.http.get(this.baseUrl + 'workspaces/' + workspaceId + '/boards/' + boardId, { withCredentials: true });
  }

  createBoard(workspaceId: string, value: { name: string; description?: string }) {
    return this.http.post(this.baseUrl + 'workspaces/' + workspaceId + '/boards', value, { withCredentials: true });
  }

  moveCard(workspaceId: string, boardId: string, cardId: string, value: any) {
    return this.http.put(this.baseUrl + 'workspaces/' + workspaceId + '/boards/' + boardId + '/cards/' + cardId + '/move', value, { withCredentials: true });
  }
}
