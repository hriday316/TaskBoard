import { HttpClient } from '@angular/common/http';
import { inject, Injectable, Service } from '@angular/core';
import { environment } from '../../environment';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
    private http = inject(HttpClient);
    private baseUrl = environment.apiUrl;

    getComments(cardId: string) {
        return this.http.get(this.baseUrl + 'cards/' + cardId + '/comments', { withCredentials: true });
    }

    createComment(cardId: string, workspaceId: string, content: string) {
        return this.http.post(this.baseUrl + 'cards/' + cardId + '/comments', { workspaceId, content }, { withCredentials: true });
    }

}
