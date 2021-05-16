import { Component, Inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http'
import { Observable, throwError } from 'rxjs';
import { catchError, map, retry } from 'rxjs/operators';
import { rejects } from 'assert';

@Component({
  selector: 'app-world-time',
  templateUrl: './world-time.component.html',
  styleUrls: ['./world-time.component.css']
})

export class WorldTimeComponent {
  public time: Time;
  public timezones: Timezones;
  errorMsg: string;
  loading: boolean = false;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.loading = true;
    this.errorMsg = "";
    this.getTime(baseUrl).subscribe(result => {
        this.time = result;
        this.loading = false;
      }, err => {
        console.error('error caught in component');
        this.errorMsg = 'Service is not available! \
                        Please try again in a few minutes.';
        this.loading = false;
        throw err;
      });
  }

  getTime(_baseUrl: string): Observable<Time> {
    return this.http.get<Time>(_baseUrl + 'api')
  }

  getTimezones(_baseUrl: string): Observable<Timezones> {
    return this.http.get<Timezones>(_baseUrl + 'api/timezones')
  }

  getPlace(): string {
    if (this.time.region === "") {
      return `${this.time.area}\\${this.time.location}`
    }
    return `${this.time.area}\\${this.time.location}\\${this.time.region}`
  }
}

interface Time {
  datetime: Date,
  area: string,
  location: string,
  region: string
}

interface Timezones {
  timezones: string[]
}
  // unknown?
  // getTime(): Observable<unknown> {
  //   return this.http
  //             .get(this.baseUrl + 'timezones')
  //             .pipe(
  //               catchError(() => {
  //                 const errMsg = 'Service is not available. \
  //                                 Please try again in a few seconds.';
  //                 return throwError(errMsg);
  //               })
  //             );
  // }