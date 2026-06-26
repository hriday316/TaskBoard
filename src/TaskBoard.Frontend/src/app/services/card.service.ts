import { inject, Injectable } from '@angular/core';
import { environment } from '../../environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CardService {
    private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;

    createCard(workspaceId: string, boardId: string, columnId: string, laneId: string, formData: FormData) {
        console.log('Creating card with data:', formData.entries());
         
        return this.http.post( this.baseUrl + 'workspaces/' + workspaceId + '/boards/' + boardId + "/cards", formData, { withCredentials: true });
    }

    getCard(workspaceId: string, boardId: string, cardId: string) {
        return this.http.get(this.baseUrl + 'workspaces/' + workspaceId + '/boards/' + boardId + '/cards/' + cardId, { withCredentials: true });
    }

}
