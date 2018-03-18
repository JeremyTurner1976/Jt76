import { TestBed, inject } from "@angular/core/testing";

import { LogFileService } from "./log-file.service";

describe("LogFileService", () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LogFileService]
    });
  });

  it("should be created", inject([LogFileService], (service: LogFileService) => {
    expect(service).toBeTruthy();
  }));
});
