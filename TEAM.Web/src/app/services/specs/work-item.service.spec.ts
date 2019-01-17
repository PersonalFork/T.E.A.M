import { TestBed } from '@angular/core/testing';
import { WorkItemService } from '../work-item.service';


describe('WorkItemService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WorkItemService = TestBed.get(WorkItemService);
    expect(service).toBeTruthy();
  });
});
