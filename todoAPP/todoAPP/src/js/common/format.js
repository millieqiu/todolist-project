import dayjs from 'dayjs';
import utc from 'dayjs/plugin/utc'
import timezone from 'dayjs/plugin/timezone'

dayjs.extend(utc)
dayjs.extend(timezone)

const utcTime = dayjs().utc();
const taiwanTime = utcTime.tz('Asia/Taipei');

export const formatDateTime = function (value) {
  return taiwanTime.format("YYYY-MM-DD HH:mm");
}