
const contentEndpointKey = "LarcanumCds:ContentEndpoint";

export class ConfigurationService {
  public get contentEndpoint(): string {
    return localStorage.getItem(contentEndpointKey) || window.location.origin;
  }
}
