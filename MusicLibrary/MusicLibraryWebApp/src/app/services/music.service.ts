import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IPathData } from '../shared/interfaces/music.interface';
import { HttpResponse } from 'selenium-webdriver/http';

@Injectable({
  providedIn: 'root'
})

export class MusicService {
  constructor(private http: HttpClient) {
  }

  addPaths(data: IPathData): Observable<HttpResponse> {
    return this.http.put('api/root/Read', data);
  }

  getPaths(): Observable<HttpResponse> {
    return this.http.get('api/root/GetPaths');
  }
}
