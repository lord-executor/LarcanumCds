import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable()
export class LarcanumCdsService {
  public constructor(private readonly httpClient: HttpClient) {
  }

  public getBySlug(slug: string): Observable<Object> {
    return this.httpClient.get(`https://localhost:7146/content/${slug}`)
  }

  public getImageUrl(imagePath: string): string {
    return `https://localhost:7146/images/${imagePath}?quality=80&height=200`;
  }
}
