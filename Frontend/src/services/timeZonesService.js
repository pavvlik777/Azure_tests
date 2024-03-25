import axios from "axios";

class TimeZonesService {
  getTimeZonesAsync() {
    return axios.get(process.env.VUE_APP_TIME_APP_API_BASEPATH + `/time`);
  }

  getTimeZoneByIdAsync(zoneId) {
    return axios.get(
      process.env.VUE_APP_TIME_APP_API_BASEPATH + `/time/${zoneId}`
    );
  }

  createTimezoneAsync(displayName, utcOffsetMinutes, ttl) {
    const request = {
      displayName: displayName,
      utcOffsetMinutes: utcOffsetMinutes,
      ttl: ttl,
    };

    return axios.post(
      process.env.VUE_APP_TIME_APP_API_BASEPATH + `/time`,
      request
    );
  }

  updateTimezoneAsync(zoneId, displayName, utcOffsetMinutes) {
    const request = {
      displayName: displayName,
      utcOffsetMinutes: utcOffsetMinutes,
    };

    return axios.put(
      process.env.VUE_APP_TIME_APP_API_BASEPATH + `/time/${zoneId}`,
      request
    );
  }

  deleteTimezoneAsync(zoneId) {
    return axios.delete(
      process.env.VUE_APP_TIME_APP_API_BASEPATH + `/time/${zoneId}`
    );
  }

  calculateTimeZonesDiffAsync(firstId, secondId) {
    return axios.post(
      process.env.VUE_APP_TIME_APP_API_BASEPATH +
        `/time/diff/${firstId}/${secondId}`
    );
  }
}

export default TimeZonesService;
