import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ConfigurationService } from "./configuration.service";

@Injectable()
export class LarcanumCdsService {
  public constructor(
    private readonly httpClient: HttpClient,
    private readonly configService: ConfigurationService
  ) {
  }

  public getBySlug(slug: string): Observable<Object> {
    return this.httpClient.get(`${this.configService.contentEndpoint}/content/${slug}`)
  }

  public getImageUrl(imagePath: string): string {
    return `${this.configService.contentEndpoint}/images/${imagePath}?quality=80&height=200`;
  }
}
