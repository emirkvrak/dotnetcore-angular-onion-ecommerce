import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class HttpClientService {
  constructor(
    private httpClient: HttpClient,
    @Inject('baseUrl') private baseUrl: string
  ) {}

  private url(requestParamater: Partial<RequestParamaters>): string {
    return `${
      requestParamater.baseUrl ? requestParamater.baseUrl : this.baseUrl
    }/${requestParamater.controller}${
      requestParamater.action ? `/${requestParamater.action}` : ''
    }`;
  }

  get<T>(
    requestParamater: Partial<RequestParamaters>,
    id?: string
  ): Observable<T> {
    let url: string;

    if (requestParamater.fullEndpoint) {
      url = requestParamater.fullEndpoint;
    } else {
      url = `${this.url(requestParamater)}${id ? `/${id}` : ''}${
        requestParamater.queryString ? `?${requestParamater.queryString}` : ''
      }`;
    }

    return this.httpClient.get<T>(url, { headers: requestParamater.headers });
  }

  post<T>(
    requestParamater: Partial<RequestParamaters>,
    body: Partial<T>
  ): Observable<T> {
    let url: string = '';

    if (requestParamater.fullEndpoint) {
      url = requestParamater.fullEndpoint;
    } else
      url = `${this.url(requestParamater)}${
        requestParamater.queryString ? `?${requestParamater.queryString}` : ''
      }`;

    return this.httpClient.post<T>(url, body, {
      headers: requestParamater.headers,
    });
  }

  put<T>(
    requestParamater: Partial<RequestParamaters>,
    body: Partial<T>
  ): Observable<T> {
    let url: string = '';
    if (requestParamater.fullEndpoint) {
      url = requestParamater.fullEndpoint;
    } else
      url = `${this.url(requestParamater)}${
        requestParamater.queryString ? `?${requestParamater.queryString}` : ''
      }`;

    return this.httpClient.put<T>(url, body, {
      headers: requestParamater.headers,
    });
  }

  delete<T>(
    requestParamater: Partial<RequestParamaters>,
    id: string
  ): Observable<T> {
    let url: string = '';
    if (requestParamater.fullEndpoint) {
      url = requestParamater.fullEndpoint;
    } else
      url = `${this.url(requestParamater)}/${id}${
        requestParamater.queryString ? `?${requestParamater.queryString}` : ''
      }`;

    return this.httpClient.delete<T>(url, {
      headers: requestParamater.headers,
    });
  }
}

export class RequestParamaters {
  controller?: string;
  action?: string;
  queryString?: string;
  headers?: HttpHeaders;
  baseUrl?: string;
  fullEndpoint?: string;
}
