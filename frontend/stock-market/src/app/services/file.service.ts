import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FileService {

  constructor(private http: HttpClient) { }

  uploadImage(file: File) {
    const formData: FormData = new FormData();
    formData.append('file', file);
    console.log(formData);

    const options = { responseType: 'text' as 'json' };

    return this.http.post<string>("https://localhost:7193/File/UploadImage", formData, options);
  }

}
